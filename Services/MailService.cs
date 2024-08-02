using CourseManagementSystem.Helpers;
using CourseManagementSystem.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CourseManagementSystem.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings mailSettings;
        private readonly ICourseService courseService;
        private readonly IConfiguration configuration;

        public MailService(IOptions<MailSettings> mailSettings, ICourseService courseService, IConfiguration configuration)
        {
            this.mailSettings = mailSettings.Value;
            this.courseService = courseService;
            this.configuration = configuration;
        }

        public async Task SendMail(string mailTo,string subject, string body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailSettings.Email),
                Subject = subject,
                
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(mailSettings.DisplayName,mailSettings.Email));

            var myPassword = configuration["MaiSettings_Password"];

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSettings.Email, myPassword);
            
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
                
        }

        public async Task AssignmentNotification(int courseId,Assignment assignment)
        {
            var course = courseService.GetCourseById(courseId);
            foreach (var student in course.Students)
            {
                string subject = $"New Assignment annoucment";
                string body = $"A new assignment was added to course {course.Name} check it out before deadline";
                await SendMail(student.Email, subject, body);
            }

        }

        public async Task AssignmentMarkedNotification(int courseId, AssignmentSubmission assignment)
        {
            string subject = "Assignment Marked";
            string body = $"Your submmision for {assignment.Assignment.Title} has been marked check it out";
            await SendMail(assignment.Student.Email, subject, body);
        }


    }
}
