using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.ViewModels
{
    public class ForgotPasswordViewModel
    {
        
        [EmailAddress]
        public string Email { get; set; }
    }
}
