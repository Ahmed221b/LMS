using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using CourseManagementSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementSystem.Controllers
{
    public class MajorController : Controller
    {
        private readonly IMajorService majorService;

        public MajorController(IMajorService majorService)
        {
            this.majorService = majorService;
        }

        public IActionResult Index()
        {
            ViewBag.Majors = majorService.GetAllMajors();
            return View();
        }

        [HttpPost]
        public IActionResult AddMajor(MajorViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (majorService.GetMajorByName(model.Name) == null)
                {
                    majorService.AddMajor(new Major { Name = model.Name });
                }
                else
                {
                    ModelState.AddModelError("Name", "A major with this name already exists");
                }
                ViewBag.Majors = majorService.GetAllMajors();
                return View("Index");
            }
            else
                ModelState.AddModelError("Name", "Something went wrong major wasn't added");
            
            ViewBag.Majors = majorService.GetAllMajors();
            return View("Index", model);
        }

        public IActionResult DeleteMajor(byte Id)
        {
            majorService.RemoveMajor(Id);
            ViewBag.Majors = majorService.GetAllMajors();
            return View("Index");
        }

        [HttpGet]
        public IActionResult EditMajor(byte Id)
        {
            var major = majorService.GetMajorById(Id);
            if (major == null)
            {
                return NotFound();
            }
            return View(new EditMajorViewModel { Id = major.Id,Name=major.Name});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMajor(EditMajorViewModel updatedMajor)
        {
            if (ModelState.IsValid)
            {
                
                majorService.UpdateMajor(new Major { Id=updatedMajor.Id,Name=updatedMajor.Name});
                ViewBag.Majors = majorService.GetAllMajors();
                return View("Index");
            }

            return View(updatedMajor);
          

        }
        

        public IActionResult Details(byte id)
        {
            var major = majorService.GetMajorById(id);
            if (major != null)
                return View(major);

            return NotFound();
        }

        [HttpPost]
        public IActionResult RemoveCourseFromMajor(byte majorId,int courseId)
        {
            majorService.RemoveCourseFromMajor(courseId, majorId);
            return View("Details",majorService.GetMajorById(majorId));
        }

        [HttpPost]
        public IActionResult RemoveStudentFromMajor(string studentId, byte majorId)
        {
            majorService.RemoveStudetFromMajor(studentId, majorId);
            return View("Details", majorService.GetMajorById(majorId));
        }
    }
}
