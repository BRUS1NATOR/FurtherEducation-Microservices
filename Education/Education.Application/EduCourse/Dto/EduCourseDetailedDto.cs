using Education.Application.Announcements.Dto;
using Education.Application.EduModules.Dto;

namespace Education.Application.EduCourses.Dto
{
    public class EduCourseDetailedDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Speciality { get; set; }
        public Guid Teacher { get; set; }
        public int Hours { get; set; }
        public int TotalMembers { get => Students.Count; }
        public string Image { get; set; }

        public List<AnnouncementPreviewDto> Announcements { get; set; }
        public List<ModulePreviewDto> Modules { get; set; }
        public List<Guid> Students { get; set; }
    }
}
