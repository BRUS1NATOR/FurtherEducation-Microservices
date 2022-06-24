using Education.Domain.EduQuizes;
using Education.Domain.EduTests;
using FurtherEducation.Common.Helpers;
using FurtherEducation.Common.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Education.Application.EduQuizes.Dto
{
    public class EduQuizResultDto
    {
        public string? Id { get; set; }
        public string? TestId { get; set; }
        public Guid UserId { get; set; }
        public bool IsFinished { get; set; }
        public int Score { get; set; }
        public DateTime AnswerGivenAt { get; set; }

        [BsonIgnore]
        public DateTime AnswerStartedAt
        {
            get
            {
                return MongoHelper.Parse(Id).CreationTime;
            }
        }
        public List<QuizAnswer> Answers { get; set; }
        public List<EduTestQuestion> Questions { get; set; }
    }

    public class EduQuizResultListDto
    {
        public PagedList<EduQuizResultDto> Answers { get; set; }
    }
}
