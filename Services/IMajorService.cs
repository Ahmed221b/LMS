using CourseManagementSystem.Models;

namespace CourseManagementSystem.Services
{
    public interface IMajorService
    {
        Major GetMajorById(byte Id);
        Major GetMajorByName(string Name);
        List<Major> GetAllMajors();
        Major AddMajor(Major major);
        void RemoveMajor(byte Id);

        void UpdateMajor(Major major);

        Major GetStudentMajor(string studentId);

        void RemoveCourseFromMajor(int courseId, byte majorId);
        void RemoveStudetFromMajor(string studentId, byte majorId);

    }
}