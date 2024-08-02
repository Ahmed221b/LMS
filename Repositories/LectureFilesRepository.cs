
using CourseManagementSystem.Data;
using CourseManagementSystem.Models;

namespace CourseManagementSystem.Repositories
{
    public class LectureFilesRepository : ILectureFilesRepository
    {
        private readonly ApplicationDbContext dbContext;
        public LectureFilesRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddLecture(LectureFile newLecture)
        {
            dbContext.LectureFile.Add(newLecture);
            Save();
        }

        public void DeleteLecture(LectureFile lecture)
        {
            if (lecture != null)
            {
                dbContext.LectureFile.Remove(lecture);
                Save();
            }
        }

        public LectureFile GetLectureById(int Id)
            => dbContext.LectureFile.Find(Id);

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
