using CourseManagementSystem.Models;
using CourseManagementSystem.ViewModels;

namespace CourseManagementSystem.Services
{
    public interface IAssignmentService
    {
        void AddAssignment(Assignment assignment);

        Assignment GetAssignment(int id);

        void DeleteAssignment(Assignment assignment);

        void UpdateAssignment(Assignment assignment);

        void AddAssignmentSubmission(AssignmentSubmission assignmentSubmission);

        AssignmentSubmission GetAssignmentSubmission(int AssignmentId, int CourseId, string StudentId);

        void MarkAssignment(MarkAssignmemtViewModel viewModel);

    }
}
