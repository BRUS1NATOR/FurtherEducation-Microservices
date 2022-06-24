using MongoDB.Bson;

namespace Education.Domain.EduTests
{
    public interface IEduTestQuestionRepository
    {
        Task<IEnumerable<EduTestQuestion>> SetQuestionsAsync(ObjectId testId, IEnumerable<EduTestQuestion> testQuestions);
        Task<IEnumerable<EduTestQuestion>> GetQuestionsAsync(ObjectId testId);
        Task<IEnumerable<EduTestQuestion>> GetQuestionsAsync(ObjectId testId, IEnumerable<Guid> questionsIds);
    }
}