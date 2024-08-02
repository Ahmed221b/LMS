namespace CourseManagementSystem.Models
{
    public class PrerequisiteCourse
    {
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public int PrerequisiteId { get; set; }
        public virtual Course Prerequisite { get; set; }
    }
}