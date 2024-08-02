namespace CourseManagementSystem.ViewModels
{
    public class EditAssignmentViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Deadline { get; set; }

        public int Marks { get; set; }

        public string? CourseName { get; set; }
    }

}
