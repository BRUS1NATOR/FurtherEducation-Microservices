using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;

namespace Education.Application.EduTaskAnswers.Commands
{
    public class RateEduTaskAnswerCommand : ICommand
    {
        [MongoObjectId]
        public string? Id { get; set; }
        public int Score { get; set; }
    }
}
