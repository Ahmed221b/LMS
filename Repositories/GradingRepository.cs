using CourseManagementSystem.Data;
using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CourseManagementSystem.Repositories
{
    public class GradingRepository : IGradingRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICourseService courseService;
        private float f4;

        public GradingRepository(ApplicationDbContext dbContext,ICourseService courseService)
        {
            this.courseService = courseService;
            this.dbContext = dbContext;
        }

        public void AddGrade(Grade newGrade)
        {
            if (SumOfGrades(newGrade) > 60)
                newGrade.Passed = true;

            CalculateGPAofaCourse(newGrade);
            CalculateCGPAofaStudent(newGrade);
            dbContext.Grades.Add(newGrade);
            Save();
        }

        public void UpdateGrade(Grade grade)
        {
            if (SumOfGrades(grade) > 60)
                grade.Passed = true;

            CalculateGPAofaCourse(grade);
            CalculateCGPAofaStudent(grade);
            dbContext.Update(grade);
            Save();
        }

        public Grade GetGrade(string studentId, int courseId)
        {
            return dbContext.Grades.Include(p=>p.Course).Include(p => p.Student).FirstOrDefault(p => p.CourseId == courseId && p.StudentId == studentId);
        }

        public void DeleteGrade(Grade Grade)
        {
            dbContext.Grades.Remove(Grade);
            Save();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }


        public float SumOfGrades(Grade grade)
            => grade.Final + grade.Assignment + grade.Midterm + grade.Practical; 
        

        public void CalculateGPAofaCourse(Grade grade)
        {
            var totalMarks = SumOfGrades(grade); 
            grade.GPA = (totalMarks / 100) * 4;
        }

        public void CalculateCGPAofaStudent(Grade grade)
        {
            var student = dbContext.Students.Include(p => p.Courses).Include(p => p.Grades).FirstOrDefault(p => p.Id == grade.StudentId);
            var sumOfGPA = student.Grades.Sum(p => p.GPA);
            student.CGPA = sumOfGPA / student.Courses.Count();

        }
      
    }
}
