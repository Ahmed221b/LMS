using CourseManagementSystem.Repositories;

namespace CourseManagementSystem.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrolmentRepository enrolmentRepository;
        public EnrollmentService(IEnrolmentRepository enrolmentRepository)
        {
            this.enrolmentRepository = enrolmentRepository;
        }

        public bool ValidateEnrollment(string studentId, int courseId)
            => enrolmentRepository.ValidateEnrollment(studentId, courseId);

        public void DropStudentFromCourse(string studentId, int courseId)
        {
            enrolmentRepository.DropStudentFromCourse(studentId, courseId);
        }

        public void EnrollToCourse(string studentId, int courseId)
        {
            enrolmentRepository.EnrollToCourse(studentId, courseId);
        }
    }
}
