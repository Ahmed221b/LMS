using CourseManagementSystem.Data;
using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementSystem.Repositories
{
    public class EnrollmentRepository : IEnrolmentRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICourseService courseService;
        private readonly IGradingService gradingService;
        

        public EnrollmentRepository(ApplicationDbContext dbContext,ICourseService courseService,IGradingService gradingService)
        {
            this.dbContext = dbContext;
            this.courseService = courseService;
            this.gradingService = gradingService;
        }

        public bool ValidateEnrollment(string studentId, int courseId)
        {
            var studentCourses = courseService.GetStudentCourses(studentId);
            var coursePrerequisites = courseService.GetCoursePrerequisites(courseId);
            bool valid = true;
            foreach(var course in coursePrerequisites)
            {
                var grade = gradingService.GetGrade(studentId, course.Id);
                
                if (!studentCourses.Contains(course) || grade == null || !grade.Passed)
                    valid = false;
            }
            
            
            return valid;
        }

        public void DropStudentFromCourse(string studentId, int courseId)
        {
            var course = courseService.GetCourseById(courseId);
            if (course != null)
            {
                var student = course.Students.FirstOrDefault(p => p.Id == studentId);
                var grade = gradingService.GetGrade(studentId, courseId);
                if (grade != null)
                {
                    gradingService.DeleteGrade(grade);
                }
                if (student != null)
                {
                    course.Students.Remove(student);
                    Save();
                }
            }
        }

        public void EnrollToCourse(string studentId, int courseId)
        {
            var course = courseService.GetCourseById(courseId);
            if (course != null)
            {
                var student = dbContext.Students.Include(s => s.Courses).FirstOrDefault(p => p.Id == studentId);
                if (student != null)
                {
                    student.Courses.Add(course);
                    Save();
                }
            }
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
