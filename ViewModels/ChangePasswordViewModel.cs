using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Display(Name ="New Password")]
        [DataType(DataType.Password)]

        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Password isn't matching")]
        public string ConfirmPassword { get; set;}
    }
}
