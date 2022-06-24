using Education.Domain.EduTests;
using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using System.ComponentModel.DataAnnotations;

namespace Education.Application.EduTests.Commands
{
    public class UpdateEduTestCommand : ICommand
    {
        [MongoObjectId]
        [Required]
        public string? Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        public EduTestSettings TestSettings { get; set; }
        [Required]
        public int Order { get; set; }
    }
}
