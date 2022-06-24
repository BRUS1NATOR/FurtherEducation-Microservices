using Profile.Domain.Diary;

namespace Profile.Application.Diary.Dto
{
    public class StudentDiaryDtoPreview
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime StartedAt { get; set; }
        public StudentDiaryStatus Status { get; set; }
    }
}
