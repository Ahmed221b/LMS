using CourseManagementSystem.Models;
using CourseManagementSystem.Repositories;
using CourseManagementSystem.ViewModels;

namespace CourseManagementSystem.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository assignmentRepository;
        public AssignmentService(IAssignmentRepository assignmentRepository)
        {
            this.assignmentRepository = assignmentRepository;
        }
        public void AddAssignment(Assignment assignment)
        {
            assignmentRepository.AddAssignment(assignment);
        }

        public void AddAssignmentSubmission(AssignmentSubmission assignmentSubmission)
        {
            assignmentRepository.AddAssignmentSubmission(assignmentSubmission);
        }

        public void DeleteAssignment(Assignment assignment)
        {
            assignmentRepository.DeleteAssignment(assignment);
        }

        public Assignment GetAssignment(int id)
        {
            return assignmentRepository.GetAssignment(id);
        }

        public AssignmentSubmission GetAssignmentSubmission(int AssignmentId, int CourseId, string StudentId)
        {
            return assignmentRepository.GetAssignmentSubmission(AssignmentId, CourseId, StudentId);
        }

        public void MarkAssignment(MarkAssignmemtViewModel viewModel)
        {
            assignmentRepository.MarkAssignment(viewModel);
        }

        public void UpdateAssignment(Assignment assignment)
        {
            assignmentRepository.UpdateAssignment(assignment);
        }
    }
}
