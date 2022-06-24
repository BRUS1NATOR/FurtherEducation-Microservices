using Education.Application.Announcements.Dto;

namespace Education.Application.EduTasks.Dto
{
    public class EduTaskPreviewDto : AnnouncementPreviewDto
    {
        public DateTime ExpirationDate { get; set; }
    }
}
