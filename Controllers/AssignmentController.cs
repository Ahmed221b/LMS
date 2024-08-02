using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using CourseManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;

namespace CourseManagementSystem.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {

        private readonly ICourseService courseService;
        private readonly IMajorService majorService;
        private readonly IAssignmentService assignmentService;
        private readonly IGradingService gradingService;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;


        public AssignmentController(IWebHostEnvironment hostingEnvironment, IAssignmentService assignmentService, IMajorService majorService, ICourseService courseService, UserManager<ApplicationUser> userManager, IGradingService gradingService)
        {
            this.majorService = majorService;
            this.courseService = courseService;
            this.userManager = userManager;
            this.assignmentService = assignmentService;
            this.hostingEnvironment = hostingEnvironment;
            this.gradingService = gradingService;
        }

        [Authorize(Roles = ("Instructor"))]
        public async Task<IActionResult> Index(string majorName)
        {
            majorName = majorName ?? "All";
            var courses = new List<Course>();
            if (majorName == "All")
                courses = courseService.GetAllCourses();
            else
                courses = courseService.GetCoursesByMajor(majorName);

            var user = await userManager.GetUserAsync(User);
            ViewBag.Majors = majorService.GetAllMajors();
            ViewBag.SelectedMajor = majorName;
            return View("../Course/Index", courses.Where(p => p.InstructorId == user.Id).ToList());
        }

        [Authorize(Roles = ("Instructor"))]
        [HttpGet]
        public IActionResult AddAssignment(int Id)
        {
            var course = courseService.GetCourseById(Id);
            if (course == null)
                return NotFound();
            var newAssignment = new AddAssignmentViewModel()
            {
                CourseId = Id,
            };
            return View(newAssignment);
        }

        [Authorize(Roles = ("Instructor"))]
        [HttpPost]
        public IActionResult AddAssignment(AddAssignmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newAssignment = new Assignment()
                {
                    Title = viewModel.Title,
                    Deadline = viewModel.Deadline,
                    Description = viewModel.Description,
                    Marks = viewModel.Marks,
                    CourseId = viewModel.CourseId
                };
                assignmentService.AddAssignment(newAssignment);
                return RedirectToAction("Details", new { Id = viewModel.CourseId });

            }

            return View(viewModel);
        }


        public IActionResult AssignmentDetails(int Id)
        {
            var assignment = assignmentService.GetAssignment(Id);
            if (assignment is null)
                return NotFound();

            return View(assignment);
        }

        [Authorize(Roles = ("Instructor"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAssignment(int Id)
        {
            var assignment = assignmentService.GetAssignment(Id);
            if (assignment == null)
                return NotFound();

            assignmentService.DeleteAssignment(assignment);
            return RedirectToAction("Details", new { Id = assignment.CourseId });
        }

        [Authorize(Roles = ("Instructor"))]
        [HttpGet]
        public IActionResult EditAssignment(int Id)
        {
            var assignment = assignmentService.GetAssignment(Id);
            if (assignment is null)
                return NotFound();

            return View(new EditAssignmentViewModel
            {
                CourseName = assignment.Course.Name,
                Id = assignment.Id,
                Title = assignment.Title,
                Deadline = assignment.Deadline,
                Description = assignment.Description,
                Marks = assignment.Marks,

            });
        }

        [Authorize(Roles = ("Instructor"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAssignment(EditAssignmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var updatedAssignment = assignmentService.GetAssignment(viewModel.Id);
                if (updatedAssignment == null)
                    return NotFound();

                updatedAssignment.Title = viewModel.Title;
                updatedAssignment.Deadline = viewModel.Deadline;
                updatedAssignment.Description = viewModel.Description;
                updatedAssignment.Marks = viewModel.Marks;


                assignmentService.UpdateAssignment(updatedAssignment);
                return RedirectToAction("Details", new { Id = updatedAssignment.CourseId });

            }
            return View(viewModel);
        }

        [Authorize(Roles = ("Instructor"))]
        public IActionResult Details(int Id)
        {
            return RedirectToAction("Details", "Course", new { id = Id });
        }

        [HttpGet]
        public IActionResult AddSubmission(int courseId, int AssignmentId)
        {
            var course = courseService.GetCourseById(courseId);
            if (course is null)
                return NotFound();

            var assignment = course.Assignments.FirstOrDefault(p => p.Id == AssignmentId);
            var viewModel = new AddSubmissionViewModel
            {
                CourseId = courseId,
                StudentId = userManager.GetUserId(User),
                AssignmentId = AssignmentId,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubmission(AddSubmissionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.SubmissionFile.ContentType != "application/pdf")
                {
                    ModelState.AddModelError("", "Only PDF files are allowed");
                    return View(viewModel);
                }
                var assignment = assignmentService.GetAssignment(viewModel.AssignmentId);

                if (assignment.Deadline < DateTime.Now) 
                {
                    ModelState.AddModelError("", "Deadline has passed you can't submit now!");
                    return View(viewModel);
                }
                var assignmentSubmission = assignmentService.GetAssignmentSubmission(viewModel.AssignmentId, viewModel.CourseId, viewModel.StudentId);
                if (assignmentSubmission is not null)
                {
                    ModelState.AddModelError("", "You've already submmited your solution watch out for mark");
                    return View(viewModel);
                }
                var uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.SubmissionFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.SubmissionFile.CopyToAsync(fileStream);
                }

                var newSubmission = new AssignmentSubmission
                {
                    CourseId = viewModel.CourseId,
                    StudentId = viewModel.StudentId,
                    AssignmentId = viewModel.AssignmentId,
                    FilePath = filePath,
                    FileName = viewModel.SubmissionFile.FileName,
                    FileType = viewModel.SubmissionFile.ContentType,
                    SubmissionDate = DateTime.Now
                };
                assignmentService.AddAssignmentSubmission(newSubmission);
            }
            return RedirectToAction("AssignmentDetails", new { Id = viewModel.AssignmentId});
        }

        [HttpGet]
        public IActionResult MarkAssignment(int AssignmentId, string StudentId, int CourseId)
        {
            var assignmnt = assignmentService.GetAssignmentSubmission(AssignmentId, CourseId, StudentId);
            if (assignmnt is null)
                return NotFound();

            var studentName = assignmnt.Student.FirstName + " " + assignmnt.Student.LastName;

            var viewModel = new MarkAssignmemtViewModel
            {
                AssignmentId = AssignmentId,
                CourseId = CourseId,
                StudentId = StudentId,
                FileName = assignmnt.FileName,
                Mark = 0,
                StudentName = studentName,
                CourseName = assignmnt.Course.Name
                
            };
            
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkAssignment(MarkAssignmemtViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                assignmentService.MarkAssignment(viewModel);
                return RedirectToAction("AssignmentDetails", new { id = viewModel.AssignmentId });

            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult OpenSubmissionFile(int AssignmentId,int CourseId,string StudentId)
        {
            var assignmentSubmission = assignmentService.GetAssignmentSubmission(AssignmentId,CourseId,StudentId);
            if (assignmentSubmission == null)
                return NotFound();

            var filePath = Path.Combine(hostingEnvironment.WebRootPath, assignmentSubmission.FilePath.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, assignmentSubmission.FileType, assignmentSubmission.FileName);
        }


        [HttpGet]
        public IActionResult AssignmentResult(int AssignmentId,int CourseId,string StudentId)
        {
            var assignmentSumission = assignmentService.GetAssignmentSubmission(AssignmentId, CourseId, StudentId);
            if (assignmentSumission is null)
                return NotFound();

            var viewModel = new AssignmentResultViewModel
            {
                Mark = assignmentSumission.Mark,
                Notes = assignmentSumission.Notes,
                AssignmentTitle = assignmentSumission.Assignment.Title,
            };

            return View(viewModel);
        }

    }
}   
