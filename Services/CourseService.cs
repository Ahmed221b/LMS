using CourseManagementSystem.Models;
using CourseManagementSystem.Repositories;

namespace CourseManagementSystem.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository  courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public bool AddCourse(Course course)
        {
            return courseRepository.AddCourse(course);
        }


        public List<Course> GetAllCourses()
            => courseRepository.GetAllCourses();


        public Course GetCourseById(int id)
            =>courseRepository.GetCourseById(id);

        public List<Course> GetCoursePrerequisites(int courseId)
            =>courseRepository.GetCoursePrerequisites(courseId);
        

        public List<Course> GetCoursesByIds(List<int> courseIds)
            =>courseRepository.GetCoursesByIds(courseIds);
        

        public List<Course> GetCoursesByMajor(string majorName)
            =>courseRepository.GetCoursesByMajor(majorName);

        public List<PrerequisiteCourse> GetPrerequisiteCoursesByIds(List<int> courseIds, Course course)
        {
            return courseRepository.GetPrerequisiteCoursesByIds(courseIds, course);
        }

        public List<Course> GetStudentCourses(string studentId)
        {
            return courseRepository.GetStudentCourses(studentId);
        }

        public void RemoveCourse(int Id)
        {
            courseRepository.RemoveCourse(Id);
        }

        public void UpdateCourse(Course course)
        {
            courseRepository.UpdateCourse(course);
        }

    }
}
