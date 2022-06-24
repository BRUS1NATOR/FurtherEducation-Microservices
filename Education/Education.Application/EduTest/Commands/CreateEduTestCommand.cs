using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using System.ComponentModel.DataAnnotations;

namespace Education.Application.EduTests.Commands
{
    public class CreateEduTestCommand : ICommand
    {
        [MongoObjectId]
        [Required]
        public string? ModuleId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
