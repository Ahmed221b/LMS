namespace CourseManagementSystem.Models
{
    public class LectureFile
    {
        public int Id { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
    }

}
