namespace CourseManagementSystem.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public Student Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public float Assignment { get; set; }
        public float Midterm { get; set; }
        public float Practical { get; set; }
        public float Final { get; set; }

        public float GPA { get; set; }

        public bool Passed { get; set; }
    }
}
