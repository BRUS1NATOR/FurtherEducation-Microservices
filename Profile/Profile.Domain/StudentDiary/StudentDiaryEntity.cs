using Profile.Domain.Profile;

namespace Profile.Domain.Diary
{
    public class StudentDiaryEntity
    {
        //UNIQUE - CourseID from Education + USER ID
        public string CourseId { get; set; }
        public Guid UserId { get; set; }
        public ProfileEntity UserProfile { get; set; }

        public string CourseName { get; set; }
        public List<DiaryRecordEntity> Records { get; set; }
        public DateTime StartedAt { get; set; }
        public StudentDiaryStatus Status { get; set; }
    }
}
