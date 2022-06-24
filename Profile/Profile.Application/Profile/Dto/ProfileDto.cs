using FurtherEducation.Common.Data.Models;
using Profile.Application.Diary.Dto;

namespace Profile.Application.Profile.Dto
{
    public class ProfileDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<StudentDiaryDtoPreview> StudentDiaries { get; set; }
        public List<TeacherDiaryDto> TeacherDiaries { get; set; }
    }
}
