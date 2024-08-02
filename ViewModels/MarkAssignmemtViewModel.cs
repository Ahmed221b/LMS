using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementSystem.ViewModels
{
    public class MarkAssignmemtViewModel
    {
        public float Mark { get; set; }

        public string? Notes { get; set; }

        public string? FileName { get; set; }

        public string? StudentName { get; set; }

        public string? CourseName { get; set; }

        public int AssignmentId { get; set; }

        public int CourseId { get; set; }

        public string StudentId { get; set; }
    }
}
