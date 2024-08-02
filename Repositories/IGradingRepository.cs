using CourseManagementSystem.Models;

namespace CourseManagementSystem.Repositories
{
    public interface IGradingRepository
    {
        void AddGrade(Grade newGrade);
        Grade GetGrade(string studentId,int courseId);

        void DeleteGrade(Grade Grade);

        void UpdateGrade(Grade grade); 

        void Save();
    }
}
