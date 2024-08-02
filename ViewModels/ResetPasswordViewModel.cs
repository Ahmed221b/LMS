using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.ViewModels
{
    public class ResetPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Token { get; set; }


        [Display(Name ="New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Passwords don't match")]
        public string ConfrimPassword { get; set; }
    }
}
