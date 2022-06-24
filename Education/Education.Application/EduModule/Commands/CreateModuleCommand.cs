using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using System.ComponentModel.DataAnnotations;

namespace Education.Application.EduModules.Commands
{
    public class CreateModuleCommand : ICommand
    {
        [MongoObjectId]
        [Required]
        public string? CourseId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
