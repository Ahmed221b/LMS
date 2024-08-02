using CourseManagementSystem.Models;

namespace CourseManagementSystem.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Major { get; set; }

        public string Instructor { get; set; }

        public int MaximumNumberOfStudents { get; set; }

        public float Assignments { get; set; } = 0;
        public float Midterm { get; set; } = 0;
        public float Practical { get; set; } = 0;
        public float Final { get; set; } = 0;

        public float GPA { get; set; } = 0;
        public List<string> Prerequisites { get; set; }
        public List<string> Dependents { get; set; }

        public List<Assignment> CourseAssignments { get; set; }
        public List<LectureFile> CourseLectureFiles { get; set; }
        public List<Student> CourseStudents { get; set; }


    }
}
