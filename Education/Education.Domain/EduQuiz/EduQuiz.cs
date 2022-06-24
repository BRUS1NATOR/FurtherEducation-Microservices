using Education.Domain.EduTests;
using FurtherEducation.Common.Data.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Education.Domain.EduQuizes
{
    public class EduQuiz : Document
    {
        public ObjectId TestId { get; set; }
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid UserId { get; set; }
        public DateTime FinishedAt { get; set; }
        public bool IsFinished { get; set; }
        [BsonElement]
        public float Score => Answers.Sum(x => x.Score) / Questions.Count;
        public List<QuizAnswer> Answers { get; set; }
        public List<EduTestQuestion> Questions { get; set; }

        [BsonIgnore]
        public int SecondsPassed { get => (int)DateTime.Now.Subtract(Id.CreationTime).TotalSeconds; }

        public int TimeLeft(int timeToSolve)
        {
            return timeToSolve - SecondsPassed;
        }
    }

    public class QuizAnswer
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid QuestionId { get; set; }
        public int[] Answer { get; set; }
        public float Score { get; set; }    // 0% - 100%
    }
}
