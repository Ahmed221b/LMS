using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public byte? MajorId { get; set; } = 0;

        public string Address { get; set; }

        [Display(Name = "Date of birth")]
        public DateOnly DateOfBirth { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confrim Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
