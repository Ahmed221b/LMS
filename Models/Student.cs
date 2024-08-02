using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementSystem.Models
{
    public class Student : ApplicationUser
    {
        public float CGPA { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }


        public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; }

        [ForeignKey("Major")]
        public byte? MajorId { get; set; }

        public virtual Major Major { get; set; }
    }
}
