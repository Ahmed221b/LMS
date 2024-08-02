using CourseManagementSystem.Models;

namespace CourseManagementSystem.Repositories
{
    public interface ICourseRepository
    {
        Course GetCourseById(int id);

        List<Course> GetAllCourses();

        List<Course> GetCoursesByMajor(string majorName);

        bool AddCourse(Course course);

        void RemoveCourse(int Id);

        void UpdateCourse(Course course);

        List<Course> GetCoursesByIds(List<int> courseIds);

        void DropStudentFromCourse(string studentId, int courseId);

        List<PrerequisiteCourse> GetPrerequisiteCoursesByIds(List<int> courseIds,Course course);

        List<Course> GetStudentCourses(string  studentId);

        List<Course> GetCoursePrerequisites(int courseId);

        void Save();




    }
}
