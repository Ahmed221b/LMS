using CourseManagementSystem.Models;

namespace CourseManagementSystem.Services
{
    public interface IGradingService
    {
        void AddGrade(Grade newGrade);

        Grade GetGrade(string studentId,int courseId);

        void DeleteGrade(Grade grade);

        void UpdateGrade(Grade newGrade);

    }
}
