using System.ComponentModel.DataAnnotations.Schema;

namespace CourseManagementSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MaxNumberOfStudents { get; set; }



        [ForeignKey("Instructor")]
        public string? InstructorId { get; set; }

        public virtual Instructor Instructor { get; set; }

        [ForeignKey("Major")]
        public byte? MajorId { get; set; }

        public virtual Major Major { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

        public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; }
        
        public virtual ICollection<PrerequisiteCourse> PrerequisiteCourses { get; set; } = new List<PrerequisiteCourse>();
        public virtual ICollection<PrerequisiteCourse> DependentCourses { get; set; }

        public virtual ICollection<LectureFile> LectureFiles { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }

    }
}
