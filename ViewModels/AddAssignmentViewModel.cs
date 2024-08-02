namespace CourseManagementSystem.ViewModels
{
    public class AddAssignmentViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Marks { get; set; }

        public DateTime Deadline { get; set; }

        public int CourseId { get; set; }
    }
}
