using CourseManagementSystem.Models;

namespace CourseManagementSystem.Repositories
{
    public interface ILectureFilesRepository
    {
        void AddLecture(LectureFile newLecture);
        LectureFile GetLectureById(int Id);
        void DeleteLecture(LectureFile lecture);

        void Save();
    }
}
