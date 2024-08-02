using CourseManagementSystem.Models;

namespace CourseManagementSystem.Services
{
    public interface ILectureFilesService
    {
        void AddLecture(LectureFile lecture);

        LectureFile GetLectureById(int Id);

        void DeleteLecture(LectureFile lecture);
    }
}
