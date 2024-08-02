using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}

