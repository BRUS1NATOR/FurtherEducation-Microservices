using FurtherEducation.Common;
using FurtherEducation.Common.CQRS.Commands;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Education.Application.EduQuizes.Commands
{
    public class UpdateEduQuizResultCommand : ICommand
    {
        [MongoObjectId]
        [Required]
        public string? Id { get; set; }
        [Required]
        public bool IsFinished { get; set; }
        public float Score { get; set; }
        [Required]
        public List<EduQuizAnswerDto> Answers { get; set; }
    }

    public class EduQuizAnswerDto
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid QuestionId { get; set; }
        public int[] Answer { get; set; }
        public float Score { get; set; }
    }
}