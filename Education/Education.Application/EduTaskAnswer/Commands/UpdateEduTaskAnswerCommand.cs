using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using System.Text.Json.Serialization;

namespace Education.Application.EduTaskAnswers.Commands
{
    public class UpdateEduTaskAnswerCommand : ICommand
    {
        [MongoObjectId]
        public string? TaskId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        public string? TextAnswer { get; set; }
        public List<AttachedFile> Files { get; set; }
    }
}
