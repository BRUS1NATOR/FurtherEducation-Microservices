using Profile.Domain.Diary;

namespace Profile.Application.Diary.Dto
{
    public class StudentDiaryDto
    {
        public string CourseId { get; set; }
        public Guid UserId { get; set; }
        public string CourseName { get; set; }
        public List<StudentDiaryRecordDto> Records { get; set; }
        public DateTime StartedAt { get; set; }
        public StudentDiaryStatus Status { get; set; }
    }
}
