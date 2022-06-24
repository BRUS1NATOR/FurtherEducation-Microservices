using FurtherEducation.Common.Models;

namespace Education.Application.EduTaskAnswers.Dto
{
    public class EduTaskAnswerListDto
    {
        public PagedList<EduTaskAnswerDto> Answers { get; set; }
    }
}
