using Education.Application.Announcements.Dto;
using Education.Domain.Enum;

namespace Education.Application.EduTasks.Dto
{
    public class EduTaskDto : AnnouncementDto
    {
        // public List<string> StudentAnswers { get; set; }
        public DateTime ExpirationDate { get; set; }
        public AnswerType AnswerType { get; set; }
    }
}
