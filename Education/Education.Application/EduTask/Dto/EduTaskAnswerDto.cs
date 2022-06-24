using Education.Domain.EduTaskAnswers;
using FurtherEducation.Common.Models;

namespace Education.Application.EduTasks.Dto
{
    public class EduTaskAnswerDto
    {
        public string Id { get; set; }
        public string TaskId { get; set; }
        public Guid UserId { get; set; }
        public string TextAnswer { get; set; }
        public List<FileAnswer> Files { get; set; }
        public int Score { get; set; }
    }

    public class EduTaskAnswerListDto
    {
        public PagedList<EduTaskAnswerDto> Answers { get; set; }
    }
}
