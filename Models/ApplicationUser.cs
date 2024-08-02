﻿using Microsoft.AspNetCore.Identity;

namespace CourseManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public DateOnly DataOfBirth { get; set; }

        public string PhoneNumber { get; set; }

    }
}
