using CourseManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repositories
{
    public interface IMajorRepository
    {
        Major GetMajorById(byte Id);
        Major GetMajorByName(string Name);
        List<Major> GetAllMajors();
        Major AddMajor(Major major);
        void RemoveMajor(byte Id);
        void UpdateMajor(Major major);

        Major GetStudentMajor(string studentId);

        void RemoveCourseFromMajor(int courseId, byte majorId);
        void RemoveStudentFromMajor(string studentId, byte majorId);
    
        void Save();

    }
}
