using Education.Application.Data.Context;
using Education.Domain.EduTests;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Education.Application.Data.Repositories
{
    public class EduTestQuestionRepository : IEduTestQuestionRepository
    {
        private readonly ICourseContext _context;
        private readonly ILogger _logger;

        public EduTestQuestionRepository(ICourseContext context, ILogger<EduTestRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        //CREATE
        public async Task<IEnumerable<EduTestQuestion>> AddQuestionsAsync(ObjectId testId, IEnumerable<EduTestQuestion> testQuestions)
        {
            foreach (var q in testQuestions)
            {
                q.Id = Guid.NewGuid();
                int count = 1;
                q.Variants.ForEach(x => x.Id = count++);
            }

            var filter = Builders<EduTest>.Filter.Eq(x => x.Id, testId);
            var update = Builders<EduTest>.Update.PushEach(x => x.Questions, testQuestions);

            await _context.EduTests.UpdateOneAsync(filter, update);

            return testQuestions;
        }

        //READ
        public async Task<IEnumerable<EduTestQuestion>> GetQuestionsAsync(ObjectId testId)
        {
            var filter = Builders<EduTest>.Filter.Eq(z => z.Id, testId);

            var result = await _context.EduTests.Find(filter)
                .Project(x => x.Questions)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<EduTestQuestion>> GetQuestionsAsync(ObjectId testId, IEnumerable<Guid> questionsIds)
        {
            var filter = Builders<EduTest>.Filter.Eq(z => z.Id, testId);

            var result = await _context.EduTests.Aggregate()
                .Match(filter)
                .Project(Builders<EduTest>.Projection.Expression(t => t.Questions.FindAll(x => questionsIds.Contains(x.Id))))
                .FirstOrDefaultAsync();

            return result;
        }

        //UPDATE
        public async Task<IEnumerable<EduTestQuestion>> SetQuestionsAsync(ObjectId testId, IEnumerable<EduTestQuestion> testQuestions)
        {
            foreach (var question in testQuestions)
            {
                if (question.Id == Guid.Empty)
                {
                    question.Id = Guid.NewGuid();
                }

                int count = 1;
                question.Variants.ForEach(x => x.Id = count++);
            }

            var filter = Builders<EduTest>.Filter.Eq(x => x.Id, testId);
            var update = Builders<EduTest>.Update.Set(x => x.Questions, testQuestions);

            var updateResult = await _context.EduTests.UpdateOneAsync(filter, update);

            var res = updateResult.IsAcknowledged
                 && updateResult.ModifiedCount > 0;

            var test = await _context.EduTests
                    .Find(filter)
                    .FirstOrDefaultAsync();

            return test.Questions;
        }

        //DELETE
        public async Task<bool> RemoveQuestionsAsync(ObjectId testId, IEnumerable<EduTestQuestion> testQuestion)
        {
            var filter = Builders<EduTest>.Filter.Eq(x => x.Id, testId);
            var questionFilter = Builders<EduTestQuestion>.Filter.In(x => x.Id, testQuestion.Select(x => x.Id));

            var update = Builders<EduTest>.Update.PullFilter(x => x.Questions, questionFilter);

            var updateResult = await _context.EduTests.UpdateOneAsync(filter, update);

            return updateResult.IsAcknowledged
                 && updateResult.ModifiedCount > 0;
        }
    }
}
