using Education.Domain.Enum;
using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using System.ComponentModel.DataAnnotations;

namespace Education.Application.EduTasks.Commands
{
    public class CreateEduTaskCommand : ICommand
    {
        [MongoObjectId]
        [Required]
        public string? ModuleId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public AnswerType AnswerType { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
