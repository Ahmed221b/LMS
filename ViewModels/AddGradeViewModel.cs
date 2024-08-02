namespace CourseManagementSystem.ViewModels
{
    public class AddGradeViewModel
    {

        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? StudentName { get; set; }

        public float Assignment { get; set; }
        public float Midterm { get; set; }
        public float Practical { get; set; }
        public float Final { get; set; }
    }
}
