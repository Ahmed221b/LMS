namespace CourseManagementSystem.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Marks { get; set; }

        public DateTime Deadline { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; }

    }
}
