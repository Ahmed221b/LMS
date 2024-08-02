using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementSystem.ViewModels
{
    public class AddSubmissionViewModel
    {
        public int AssignmentId { get; set; }

        public int CourseId { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }


        public IFormFile SubmissionFile { get; set; }
    }
}
