using Profile.Domain.Diary;
using System.ComponentModel.DataAnnotations;

namespace Profile.Domain.Profile
{
    public class ProfileEntity
    {
        [Key]
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<StudentDiaryEntity>? StudentDiaries { get; set; }
        public List<TeacherDiaryEntity>? TeacherDiaries { get; set; }
        //Настройки?
    }
}
