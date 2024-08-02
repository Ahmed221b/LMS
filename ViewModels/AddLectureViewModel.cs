using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.ViewModels
{
    public class AddLectureViewModel
    {
        public int CourseId { get; set; }

        [Display(Name ="Upload lecture file")]
        public IFormFile LectureFile { get; set; }
    }
}
