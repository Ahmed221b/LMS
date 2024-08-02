using CourseManagementSystem.Models;
using CourseManagementSystem.Repositories;

namespace CourseManagementSystem.Services
{
    public class GradingService : IGradingService
    {
        private readonly IGradingRepository _repository;
        public GradingService(IGradingRepository repository)
        {
            _repository = repository;
        }

        public void AddGrade(Grade newGrade)
        {
            _repository.AddGrade(newGrade);
        }

        public void DeleteGrade(Grade grade)
        {
            _repository.DeleteGrade(grade);
        }

        public Grade GetGrade(string studentId, int courseId)
        {
            return _repository.GetGrade(studentId, courseId);
        }

        public void UpdateGrade(Grade newGrade)
        {
            _repository.UpdateGrade(newGrade);
        }
    }
}
