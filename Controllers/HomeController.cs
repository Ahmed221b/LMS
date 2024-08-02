using CourseManagementSystem.Models;
using CourseManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseManagementSystem.Controllers
{


    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles =("Student,Instructor"))]
        public IActionResult Index()
        {
            return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotFound(int statusCode)
        {
            Response.StatusCode = statusCode;
            return View();
        }

        
    }
}
