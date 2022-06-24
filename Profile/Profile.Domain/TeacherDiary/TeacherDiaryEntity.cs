using Profile.Domain.Profile;

namespace Profile.Domain.Diary
{
    public class TeacherDiaryEntity
    {
        //UNIQUE - CourseID from Education + USER ID
        public string CourseId { get; set; }
        public Guid UserId { get; set; }
        public ProfileEntity UserProfile { get; set; }

        public string CourseName { get; set; }
        public DateTime CreatedAt { get; set; }
        public TeacherDiaryStatus Status { get; set; }
    }
}
