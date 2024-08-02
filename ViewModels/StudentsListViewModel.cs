using CourseManagementSystem.Models;

namespace CourseManagementSystem.ViewModels
{
    public class StudentsListViewModel
    {
        public ICollection<Student> Students { get; set; }

        public int CourseId { get; set; }
    }
}
