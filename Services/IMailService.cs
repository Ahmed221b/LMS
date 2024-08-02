using CourseManagementSystem.Models;

namespace CourseManagementSystem.Services
{
    public interface IMailService
    {
        Task SendMail(string mailTo, string subject, string body);
        Task AssignmentNotification(int courseId,Assignment assignment);

        Task AssignmentMarkedNotification(int courseId,AssignmentSubmission assignment);
    }
}
