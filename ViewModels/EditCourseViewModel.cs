using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.ViewModels
{
    public class EditCourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Display(Name ="Maximum Number of students")]
        public int MaxNumOfStudents { get; set; }

        [Display(Name = "Major")]
        public byte? MajorId { get; set; }

        [Display(Name = "Instructor")]
        public string? InstructorId { get; set; }

        [Display(Name ="Prerequisite courses")]
        public List<int>? PrerequisiteCourses { get; set; }
    }
}
