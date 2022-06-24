using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Education.Domain.EduTests
{
    public class EduTestQuestion
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
        public string Question { get; set; }
        public List<EduTestAnswerVariant> Variants { get; set; }
        public bool MultipleAnswers { get; set; }

        public float IsCorrect(int[] answerIds)
        {
            var correctAnswers = Variants.Where(x => x.IsCorrect);
            if (!correctAnswers.Any())
            {
                return 1f;
            }
            if (MultipleAnswers)
            {
                //TODO: Проверять неправильные!!!
                var userCorrectAnswers = correctAnswers.IntersectBy(answerIds, x => x.Id);
                return (float)userCorrectAnswers.Count() / correctAnswers.Count();
            }
            else
            {
                return correctAnswers.First().Id == answerIds.First() ? 1f : 0f;
            }
        }
    }

    public class EduTestAnswerVariant
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
    }
}