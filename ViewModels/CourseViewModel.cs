using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.ViewModels
{
    public class CourseViewModel
    {
        public string Name { get; set; }

        public int MaxNumberOfStudents { get; set; }

        public byte MajorId { get; set; }

        public List<int>? SelectedCoursesIds { get; set; }
    }

}
