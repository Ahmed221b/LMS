using CourseManagementSystem.Data;
using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using CourseManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMailService mailService;
        private readonly IGradingService gradingService;
        public AssignmentRepository(ApplicationDbContext dbContext, IMailService mailService, IGradingService gradingService)
        {
            this.dbContext = dbContext;
            this.mailService = mailService;
            this.gradingService = gradingService;
        }

        public void AddAssignment(Assignment assignment)
        {
            dbContext.Assignments.Add(assignment);
            mailService.AssignmentNotification(assignment.CourseId, assignment);
            Save();
        }

        public void AddAssignmentSubmission(AssignmentSubmission newSubmission)
        {
            dbContext.AssignmentSubmissions.Add(newSubmission);
            Save();
        }

        public void UpdataAssignmentSubmission(AssignmentSubmission updatedSubmission)
        {
            dbContext.AssignmentSubmissions.Update(updatedSubmission);
            Save();
        }

        public void DeleteAssignment(Assignment assignment)
        {
            if (assignment != null)
            {
                dbContext.Assignments.Remove(assignment);
                Save();
            }
            
        }

        public Assignment GetAssignment(int id)
            => dbContext.Assignments
            .Include(p => p.Course)
            .Include(p => p.AssignmentSubmissions)
            .ThenInclude(p => p.Student)
            .FirstOrDefault(p => p.Id == id);

        public AssignmentSubmission GetAssignmentSubmission(int AssignmentId, int CourseId, string StudentId)
        {
            return dbContext.AssignmentSubmissions.Include(p=>p.Student).Include(p => p.Course).Include(p=>p.Assignment).FirstOrDefault(p => p.AssignmentId == AssignmentId && p.CourseId == CourseId && p.StudentId == StudentId);
        }

        public void MarkAssignment(MarkAssignmemtViewModel markAssignmemtViewModel)
        {
            var grade = gradingService.GetGrade(markAssignmemtViewModel.StudentId,markAssignmemtViewModel.CourseId);
            
            if (grade is null)
                gradingService.AddGrade(new Grade { Assignment = markAssignmemtViewModel.Mark });
            
            else
            {
                grade.Assignment += markAssignmemtViewModel.Mark;
                gradingService.UpdateGrade(grade);
            }

            var submisson = GetAssignmentSubmission(markAssignmemtViewModel.AssignmentId, markAssignmemtViewModel.CourseId, markAssignmemtViewModel.StudentId);
            submisson.IsMarked = true;
            submisson.Notes = markAssignmemtViewModel.Notes;
            submisson.Mark = markAssignmemtViewModel.Mark;
            UpdataAssignmentSubmission(submisson);

            mailService.AssignmentMarkedNotification(markAssignmemtViewModel.CourseId,submisson);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void UpdateAssignment(Assignment assignment)
        {
            if(assignment != null)
            {
                dbContext.Assignments.Update(assignment);
                Save();
            }
        }
    }
}
