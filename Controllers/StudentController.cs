using CourseManagementSystem.Data;
using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Controllers
{
    [Authorize(Roles ="Student")]
    public class StudentController : Controller
    {
        private readonly ICourseService courseService;
        private readonly IMajorService majorService;
        private readonly IEnrollmentService enrollmentService;
        private readonly UserManager<ApplicationUser> userManager;
        public StudentController(IEnrollmentService enrollmentService,IMajorService majorService,ICourseService courseService, UserManager<ApplicationUser> userManager)
        {
            this.courseService = courseService;
            this.userManager = userManager;
            this.majorService = majorService;
            this.enrollmentService = enrollmentService;
        }

        public IActionResult MyCourses()
        {
            var studentId = userManager.GetUserId(User);

            var studentCourses = courseService.GetStudentCourses(studentId);
            if (studentCourses is null)
                return NotFound();

            return View("../Course/Index", studentCourses);
        }

        public  IActionResult Index()
        {
            var studentId = userManager.GetUserId(User);
            
            var major = majorService.GetStudentMajor(studentId);

            var majorCourses = courseService.GetCoursesByMajor(major.Name);

            ViewBag.MajorName = major.Name;
            return View(majorCourses); 
        }

        [HttpPost]
        public IActionResult Enroll(string studentId,int courseId)
        {
            bool valid = enrollmentService.ValidateEnrollment(studentId, courseId);
            if (valid)
            {   
                enrollmentService.EnrollToCourse(studentId, courseId);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "The student didn't take all the prerequisite courses for this course." });
        }


        [HttpPost]
        public IActionResult DropCourse(int courseId,string studentId)
        {
            enrollmentService.DropStudentFromCourse(studentId,courseId);
            return RedirectToAction("MyCourses");
        }


    }
}
