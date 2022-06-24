using Profile.Domain.Diary;

namespace Profile.Application.Diary.Dto
{
    public class TeacherDiaryDto
    {
        public string CourseId { get; set; }
        public Guid UserId { get; set; }
        public string CourseName { get; set; }
        public DateTime CreatedAt { get; set; }
        public TeacherDiaryStatus Status { get; set; }
    }
}
