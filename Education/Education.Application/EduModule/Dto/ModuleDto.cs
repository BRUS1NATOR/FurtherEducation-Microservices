using Education.Application.Announcements.Dto;
using Education.Application.EduTasks.Dto;
using Education.Application.EduTests.Dto;

namespace Education.Application.EduModules.Dto
{
    public class ModuleDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public int Order { get; set; }
        public List<AnnouncementDto> Announcements { get; set; }
        public List<EduTaskPreviewDto> Tasks { get; set; }
        public List<EduTestPreviewDto> Tests { get; set; }
    }
}
