using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Models
{
    public class Major
    {
        [Key]
        public byte Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
