using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.ViewModels
{
    public class EditProfileViewModel
    {
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }


        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumer { get; set; }

        [Display(Name = "Date of birth")]
        public DateOnly DateOfBirth { get; set; }

    }
}
