using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Education.Application.EduQuizes.Commands
{
    public class FinishEduQuizCommand : ICommand
    {
        [MongoObjectId]
        [Required]
        public string? TestId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [Required]
        public List<EduQuizAnswerDto> Answers { get; set; }
    }
}
