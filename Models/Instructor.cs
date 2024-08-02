namespace CourseManagementSystem.Models
{
    public class Instructor : ApplicationUser
    {
        public virtual ICollection<Course> Courses { get; set; }
    }
}
