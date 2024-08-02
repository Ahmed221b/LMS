using CourseManagementSystem.Models;
using CourseManagementSystem.Repositories;

namespace CourseManagementSystem.Services
{
    public class MajorService : IMajorService
    {
        private readonly IMajorRepository majorRepository;

        public MajorService(IMajorRepository majorRepository)
        {
            this.majorRepository = majorRepository;
        }


        public Major AddMajor(Major major)
            => majorRepository.AddMajor(major);

        public List<Major> GetAllMajors() 
            => majorRepository.GetAllMajors();

        public Major GetMajorById(byte Id)
            => majorRepository.GetMajorById(Id);

        public Major GetMajorByName(string Name)
            => majorRepository.GetMajorByName(Name);

        public void RemoveMajor(byte Id)
        {
            majorRepository.RemoveMajor(Id);
        }

        public void UpdateMajor(Major major)
        {
            majorRepository.UpdateMajor(major);
        }

        public void RemoveCourseFromMajor(int courseId, byte majorId)
        {
            majorRepository.RemoveCourseFromMajor(courseId, majorId);
        }

        public void RemoveStudetFromMajor(string studentId, byte majorId)
        {
            majorRepository.RemoveStudentFromMajor(studentId, majorId);
        }

        public Major GetStudentMajor(string studentId)
        {
            return majorRepository.GetStudentMajor(studentId);
        }
    }
}
