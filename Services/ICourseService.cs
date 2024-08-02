using CourseManagementSystem.Models;

namespace CourseManagementSystem.Services
{
    public interface ICourseService
    {
        Course GetCourseById(int id);

        List<Course> GetAllCourses();

        List<Course> GetCoursesByMajor(string majorName);

        bool AddCourse(Course course);

        void RemoveCourse(int Id);

        List<Course> GetCoursesByIds(List<int> courseIds);

        void UpdateCourse(Course course);


        List<PrerequisiteCourse> GetPrerequisiteCoursesByIds(List<int> courseIds, Course course);

        List<Course> GetStudentCourses(string studentId);
        List<Course> GetCoursePrerequisites(int courseId);

    }
}
