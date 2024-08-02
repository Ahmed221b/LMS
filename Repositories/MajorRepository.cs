using CourseManagementSystem.Data;
using CourseManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repositories
{
    public class MajorRepository : IMajorRepository
    {
        private readonly ApplicationDbContext _context;
        public MajorRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public Major AddMajor(Major major)
        {
            _context.Majors.Add(major);
            Save();
            return major;
        }

        public List<Major> GetAllMajors()
            => _context.Majors.ToList();

        public Major GetMajorById(byte Id)
            => _context.Majors.Include(p => p.Students).Include(p => p.Courses).ThenInclude(p => p.Students).FirstOrDefault(p => p.Id == Id);
        public Major GetMajorByName(string Name)
            => _context.Majors.FirstOrDefault(x => x.Name == Name);

        public void RemoveMajor(byte Id)
        {
            var major = GetMajorById(Id);
            if (major != null)
            {
                _context.Majors.Remove(major);
                Save();
            }
        }
        public void UpdateMajor(Major updatedMajor)
        {
            
                _context.Majors.Update(updatedMajor);
                Save();
            
        }

        public void RemoveCourseFromMajor(int courseId, byte majorId)
        {
            var major = GetMajorById(majorId);
            if (major != null)
            {
                var course = major.Courses.FirstOrDefault(p => p.Id == courseId);
                if (course != null)
                {
                    course.MajorId = null;
                    Save();
                }
            }
        }

        public void RemoveStudentFromMajor(string studentId, byte majorId)
        {
            var major = GetMajorById(majorId);
            if (major != null)
            {
                var student = major.Students.FirstOrDefault(p => p.Id == studentId);
                student.MajorId = null;
                Save();
            }

        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Major GetStudentMajor(string studentId)
        {
            var student = _context.Students.Include(p => p.Major).FirstOrDefault(p => p.Id == studentId);
            return student.Major;
        }
    }
}
