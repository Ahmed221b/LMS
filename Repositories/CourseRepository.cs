using CourseManagementSystem.Data;
using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using CourseManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CourseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool AddCourse(Course newCourse)
        {
            var course = dbContext.Courses.FirstOrDefault(x => x.Name == newCourse.Name);
            if (course == null)
            { 
                dbContext.Courses.Add(newCourse);
                Save();
                return true;
            }

            return false;
        }

        public List<Course> GetAllCourses()
            => dbContext.Courses.Include(p => p.Students).Include(p => p.Instructor).Include(p=>p.Major).ToList();

        public Course GetCourseById(int id)
            => dbContext.Courses
            .Include(p => p.Major)
            .Include(p => p.Students)
            .Include(p => p.Instructor)
            .Include(p => p.PrerequisiteCourses)
            .ThenInclude(p => p.Prerequisite)
            .Include(p => p.DependentCourses)
            .ThenInclude(p => p.Course)
            .Include(p => p.Assignments)
            .Include(p => p.LectureFiles)
            .FirstOrDefault(m => m.Id == id);

        public List<Course> GetCoursesByMajor(string majorName)
            => dbContext.Courses
            .Where(m => m.Major.Name == majorName)
            .Include(p => p.Students)
            .Include(p => p.Instructor)
            .Include(p => p.Major)
            .ToList();


        public void RemoveCourse(int Id)
        {
            var course = dbContext.Courses.Find(Id);
            if (course != null)
            {
                dbContext.Courses.Remove(course);
                var dependentCourses = dbContext.PrerequisiteCourse.Where(p => p.PrerequisiteId == course.Id);
                var prerequisites = dbContext.PrerequisiteCourse.Where(p => p.CourseId == course.Id);
                dbContext.PrerequisiteCourse.RemoveRange(dependentCourses);
                dbContext.PrerequisiteCourse.RemoveRange(prerequisites);
                Save();
            }
        }

        public void UpdateCourse(Course updatedCourse)
        {
            var course = dbContext.Courses.Find(updatedCourse.Id);
            if (course != null)
            {
                dbContext.Courses.Update(updatedCourse);
                Save();
            }
        }
    

        public List<Course> GetCoursesByIds(List<int> courseIds)
            => dbContext.Courses.Where(c => courseIds.Contains(c.Id)).ToList();

        public void DropStudentFromCourse(string studentId, int courseId)
        {
            var course = GetCourseById(courseId);
            if (course != null)
            {
                var student = course.Students.FirstOrDefault(p => p.Id == studentId);
                if (student != null)
                {
                    course.Students.Remove(student);
                    Save();
                }
            }
        }

        public List<PrerequisiteCourse> GetPrerequisiteCoursesByIds(List<int> courseIds, Course course)
        {
            var prerequisites = GetCoursesByIds(courseIds);
            foreach (var prerequisite in prerequisites)
            {
                course.PrerequisiteCourses.Add(new PrerequisiteCourse
                {
                    CourseId = course.Id,
                    Course = course,
                    Prerequisite = prerequisite,
                    PrerequisiteId = prerequisite.Id
                }
                );
            }

            return (List<PrerequisiteCourse>)course.PrerequisiteCourses;
        }


        public List<Course> GetStudentCourses(string studentId)
        {
            var student = dbContext.Students.FirstOrDefault(p => p.Id == studentId);
            
            if (student != null)
            {
                return dbContext.Courses.Include(p => p.Major).Include(p=>p.Instructor).Where(p => p.Students.Contains(student)).ToList();
            }
            return null;
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public List<Course> GetCoursePrerequisites(int courseId)
        {
            var prerequisites = new List<Course>();
            var course = GetCourseById(courseId);
            foreach (var prerequisiteCourse in course.PrerequisiteCourses)
            {
                prerequisites.Add(prerequisiteCourse.Prerequisite);
            }

            return prerequisites;
        }
    }
}
