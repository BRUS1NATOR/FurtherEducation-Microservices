using Education.Domain.EduTests;
using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using System.ComponentModel.DataAnnotations;

namespace Education.Application.EduTests.Commands
{
    public class SetEduTestQuestionsCommand : ICommand
    {
        [MongoObjectId]
        [Required]
        public string? TestId { get; set; }
        public List<EduTestQuestion> Questions { get; set; }
    }
}
