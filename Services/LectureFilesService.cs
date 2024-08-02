using CourseManagementSystem.Models;
using CourseManagementSystem.Repositories;

namespace CourseManagementSystem.Services
{
    public class LectureFilesService : ILectureFilesService
    {
        private readonly ILectureFilesRepository repository;
        public LectureFilesService(ILectureFilesRepository lectureFilesRepository)
        {
            repository = lectureFilesRepository;
        }
        public void AddLecture(LectureFile lecture)
        {
            repository.AddLecture(lecture);
        }

        public void DeleteLecture(LectureFile lecture)
        {
            repository.DeleteLecture(lecture);
        }

        public LectureFile GetLectureById(int Id)
        {
            return repository.GetLectureById(Id);
        }
    }
}
