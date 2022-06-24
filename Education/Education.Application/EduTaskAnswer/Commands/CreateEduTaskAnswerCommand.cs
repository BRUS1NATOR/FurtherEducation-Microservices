using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Education.Application.EduTaskAnswers.Commands
{
    public class CreateEduTaskAnswerCommand : ICommand
    {
        [MongoObjectId]
        [Required]
        public string TaskId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }
        [Required]
        public string? TextAnswer { get; set; }
        public List<AttachedFile> Files { get; set; }
    }

    public class AttachedFile
    {
        public string? FileName { get; set; }
        public string? FileExtension { get; set; }
        public string? FileUrl { get; set; }
    }
}