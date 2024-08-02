using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using CourseManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using CourseManagementSystem.Repositories;

namespace CourseManagementSystem.Controllers
{

    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IMajorService majorService;
        private readonly IGradingService gradingService;
        private readonly ILectureFilesService lectureFilesService;
        private readonly IEnrollmentService enrollmentService;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;


        public CourseController(IEnrollmentService enrollmentService,IWebHostEnvironment hostingEnvironment,IGradingService gradingService, ICourseService courseService, IMajorService majorService, UserManager<ApplicationUser> userManager, ILectureFilesService lectureFilesService)
        {
            this.courseService = courseService;
            this.majorService = majorService;
            this.userManager = userManager;
            this.gradingService = gradingService;
            this.lectureFilesService = lectureFilesService;
            this.hostingEnvironment = hostingEnvironment;
            this.enrollmentService = enrollmentService;
        }


        public IActionResult Index(string majorName)
        {
            majorName = majorName ?? "All";
            var courses = new List<Course>();
            if (majorName == "All")
                courses = courseService.GetAllCourses();
            
            else
                courses = courseService.GetCoursesByMajor(majorName);

            ViewBag.SelectedMajor = majorName;
            ViewBag.Majors = majorService.GetAllMajors();
            return View(courses);
            
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddCourse()
        {
            var majors = majorService.GetAllMajors().Select(m => new { m.Name,m.Id });
            ViewBag.Majors = new SelectList(majors,"Id","Name");
            ViewBag.Courses = courseService.GetAllCourses();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCourse(CourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {

                var course = new Course()
                {
                    Name = courseViewModel.Name,
                    MaxNumberOfStudents = courseViewModel.MaxNumberOfStudents,
                    Major = majorService.GetMajorById(courseViewModel.MajorId),
                };

                var prerequisites = courseService.GetCoursesByIds(courseViewModel.SelectedCoursesIds);
                foreach(var prerequisite in prerequisites)
                {  
                    course.PrerequisiteCourses.Add(new PrerequisiteCourse
                    { 
                            CourseId = course.Id, 
                            Course = course, 
                            Prerequisite = prerequisite, 
                            PrerequisiteId = prerequisite.Id 
                    }
                    );
                }

                var created = courseService.AddCourse(course);

                if (!created)
                {
                    ModelState.AddModelError("", "There is a course already with this name");
                    ViewBag.Majors = new SelectList(majorService.GetAllMajors().Select(m => m.Name).ToList());
                    ViewBag.Courses = courseService.GetAllCourses();
                    return View(courseViewModel);
                }

                return RedirectToAction("Index",courseService.GetAllCourses());

            }

            ViewBag.Majors = new SelectList(majorService.GetAllMajors().Select(m => m.Name).ToList());
            ViewBag.Courses = courseService.GetAllCourses();
            return View(courseViewModel);

        }

        public IActionResult Details(int id)
        {
            var course = courseService.GetCourseById(id);
            if (course is null)
                return NotFound();

            var studentId = userManager.GetUserId(User);
            var grade = gradingService.GetGrade(studentId, id);

            var viewModel = new CourseDetailsViewModel
            {
                Id = id,
                Name = course.Name,
                Instructor = course.Instructor != null ? course.Instructor.FirstName + " " + course.Instructor.LastName : "N/A",
                Major = course.Major != null ? course.Major.Name : "N/A",
                MaximumNumberOfStudents = course.MaxNumberOfStudents,
                Prerequisites = course.PrerequisiteCourses != null ? course.PrerequisiteCourses.Select(p => p.Prerequisite.Name).ToList() : new List<string>(),
                Dependents = course.DependentCourses != null ? course.DependentCourses.Select(p => p.Course.Name).ToList() : new List<string>(),
                Assignments = grade != null ? grade.Assignment : 0,
                Midterm = grade != null ? grade.Midterm : 0,
                Practical = grade != null ? grade.Practical : 0,
                Final = grade != null ? grade.Final : 0,
                GPA = grade != null ? grade.GPA :0,
                CourseStudents = course.Students != null ? course.Students.ToList() : new List<Student>(),
                CourseAssignments = course.Assignments != null ? course.Assignments.ToList() : new List<Assignment>(),
                CourseLectureFiles = course.LectureFiles != null ? course.LectureFiles.ToList() : new List<LectureFile>()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {

            ViewBag.Instructors = await GetInstructors();
            ViewBag.Majors = new SelectList(majorService.GetAllMajors(), "Id", "Name");
            ViewBag.Courses = courseService.GetAllCourses();
            var course = courseService.GetCourseById(id);
            var prerequisitesIds = course.PrerequisiteCourses.Select(p => p.PrerequisiteId).ToList();
            return View(new EditCourseViewModel 
            {   
                Id = course.Id,
                Name=course.Name,
                MajorId=course.MajorId,
                MaxNumOfStudents = course.MaxNumberOfStudents,
                InstructorId = course.InstructorId,
                PrerequisiteCourses = prerequisitesIds
            });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(EditCourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var course = courseService.GetCourseById(viewModel.Id);
                course.Name = viewModel.Name;
                course.MajorId = viewModel.MajorId;
                course.MaxNumberOfStudents = viewModel.MaxNumOfStudents;
                course.InstructorId = viewModel.InstructorId;
                var newPrerequisites = courseService.GetCoursesByIds(viewModel.PrerequisiteCourses);
                var updatedPrerequisites = new List<PrerequisiteCourse>();
                foreach (var prerequisite in newPrerequisites)
                {
                    updatedPrerequisites.Add(new PrerequisiteCourse
                    {
                        CourseId = course.Id,
                        Course = course,
                        Prerequisite = prerequisite,
                        PrerequisiteId = prerequisite.Id
                    }
                    );
                }
                course.PrerequisiteCourses = updatedPrerequisites;
                courseService.UpdateCourse(course);
                return RedirectToAction("Index");
            }

            ViewBag.Instructors = await GetInstructors();
            ViewBag.Majors = new SelectList(majorService.GetAllMajors(), "Id", "Name");
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCourse(int id)
        {
           

            courseService.RemoveCourse(id);
            return RedirectToAction("Index",courseService.GetAllCourses());
        }

        public async Task<SelectList> GetInstructors()
        {
            var users = await userManager.Users.ToListAsync();
            var instructors = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if (await userManager.IsInRoleAsync(user, "Instructor"))
                    instructors.Add(user);
            }
            var instructorsList = instructors.Select(u => new { Id = u.Id, FullName = u.FirstName + " " + u.LastName }); ;

            return new SelectList(instructorsList, "Id", "FullName");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DropStudentFromCourse(string studentId,int courseId)
        {
            enrollmentService.DropStudentFromCourse(studentId, courseId);
            return RedirectToAction("Details", new { id = courseId });
        }


        [HttpGet]
        public async Task<IActionResult> AddGrade(int courseId,string studentId)
        {
            var grade = gradingService.GetGrade(studentId,courseId);
            var student = await userManager.FindByIdAsync(studentId) as Student;
            var course = courseService.GetCourseById(courseId);
            var gradeViewModel = new AddGradeViewModel
            {
                CourseId = grade != null? grade.CourseId : courseId,
                StudentId =grade != null? grade.StudentId : studentId,
                Assignment = grade != null ? grade.Assignment : 0,
                Midterm = grade != null ? grade.Midterm : 0,
                Practical = grade != null ? grade.Practical : 0,
                Final = grade != null ? grade.Final : 0,
                CourseName = course.Name,
                StudentName = student.FirstName + " " + student.LastName,
            };
            
            return View(gradeViewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGrade(AddGradeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var grade = gradingService.GetGrade(viewModel.StudentId, viewModel.CourseId);
                if (grade == null)
                {
                    var newGrade = new Grade
                    {
                        CourseId = viewModel.CourseId,
                        StudentId = viewModel.StudentId,
                        Assignment = viewModel.Assignment,
                        Midterm = viewModel.Midterm,
                        Final = viewModel.Final,
                        Practical = viewModel.Practical,
                    };

                    gradingService.AddGrade(newGrade);
                }
                else
                {
                    grade.Assignment = viewModel.Assignment;
                    grade.Final = viewModel.Final;
                    grade.Midterm = viewModel.Midterm;
                    grade.Practical = viewModel.Practical;
                    gradingService.UpdateGrade(grade);
                }
               
                return RedirectToAction("Details", new {id = viewModel.CourseId});
            }

            return View(viewModel);
            
        }

        [HttpGet]
        public IActionResult AddLecture(int courseId,string courseName)
        {
            var course = courseService.GetCourseById(courseId);
            if (course is null)
                return NotFound();
            var viewModel = new AddLectureViewModel
            {
                CourseId = courseId,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLecture(AddLectureViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.LectureFile.ContentType != "application/pdf")
                {
                    ModelState.AddModelError("", "Only PDF files are allowed");
                    return View(viewModel);
                }

                var uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.LectureFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.LectureFile.CopyToAsync(fileStream);
                }

                var lectureFile = new LectureFile()
                {
                    CourseId = viewModel.CourseId,
                    FilePath = "/uploads/" + uniqueFileName,
                    FileName = viewModel.LectureFile.FileName,
                    FileType = viewModel.LectureFile.ContentType,
                };

                lectureFilesService.AddLecture(lectureFile);

                return RedirectToAction("Details", new { id = viewModel.CourseId });
            }
            return View(viewModel);
        }


        [HttpGet]
        public IActionResult OpenLecture(int id)
        {
            var lecture = lectureFilesService.GetLectureById(id);
            if (lecture == null)
                return NotFound();

            var filePath = Path.Combine(hostingEnvironment.WebRootPath, lecture.FilePath.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, lecture.FileType, lecture.FileName);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLecture(int Id)
        {
            var lecture = lectureFilesService.GetLectureById(Id);
            if (lecture is null)
                return NotFound();

            lectureFilesService.DeleteLecture(lecture);
            return RedirectToAction("Details", new { id = lecture.CourseId });

        }

    }
}
