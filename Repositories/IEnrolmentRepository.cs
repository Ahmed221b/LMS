namespace CourseManagementSystem.Repositories
{
    public interface IEnrolmentRepository
    {

        void EnrollToCourse(string studentId, int courseId);

        bool ValidateEnrollment(string studentId,int courseId);

        void DropStudentFromCourse(string studentId, int courseId);


        void Save();
    }
}
