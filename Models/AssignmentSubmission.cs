using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementSystem.Models
{
    public class AssignmentSubmission
    {
        public int AssignmentId { get; set; }

        public int CourseId { get; set; }
        
        [ForeignKey("Student")] 
        public string StudentId { get; set; }

        public DateTime SubmissionDate { get; set; }

        public float Mark { get; set; }

        public string? Notes { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public bool IsMarked { get; set; } = false;

        public virtual Student Student{ get; set; }

        public virtual Course Course { get; set; }

        public virtual Assignment Assignment { get; set; }

    }


}
