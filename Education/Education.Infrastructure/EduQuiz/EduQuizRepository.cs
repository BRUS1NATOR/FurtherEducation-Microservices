using Education.Application.Data.Context;
using Education.Domain.EduQuizes;
using FurtherEducation.Common.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Education.Application.Data.Repositories
{
    public class EduQuizRepository : IEduQuizRepository
    {
        private readonly ICourseContext _context;

        public EduQuizRepository(ICourseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //CREATE
        public async Task<bool> Create(EduQuiz answer)
        {
            await _context.EduTestResult.InsertOneAsync(answer);

            return await Task.FromResult(true);
        }

        //READ
        public async Task<EduQuiz> FindAsync(ObjectId id)
        {
            var filter = Builders<EduQuiz>.Filter.Eq(z => z.Id, id);

            return await _context.EduTestResult
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<EduQuiz> FindByParentAndUserAsync(ObjectId parentId, Guid userId)
        {
            var filter = Builders<EduQuiz>.Filter.Eq(z => z.TestId, parentId)
                       & Builders<EduQuiz>.Filter.Eq(z => z.UserId, userId);

            return await _context.EduTestResult
                .Find(filter)
                .SortBy(x => x.IsFinished)
                .FirstOrDefaultAsync();
        }

        //UPDATE
        public async Task<bool> UpdateAsync(EduQuiz entity)
        {
            var old = await FindAsync(entity.Id);
            if (old == null)
            {
                return await Task.FromResult(false);
            }

            old.Answers = entity.Answers;
            old.IsFinished = entity.IsFinished;
            old.Answers = entity.Answers;
            old.FinishedAt = DateTime.UtcNow;

            await WriteToDatabase(old);

            return await Task.FromResult(true);
        }

        public async Task<bool> WriteToDatabase(EduQuiz testAnswer)
        {
            var updateResult = await _context
                  .EduTestResult
                  .ReplaceOneAsync(g => g.Id == testAnswer.Id, testAnswer);

            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        //DELETE
        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var filter = Builders<EduQuiz>.Filter.Eq(x => x.Id, id);

            var deleteResult = await _context.EduTestResult.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
              && deleteResult.DeletedCount > 0;
        }

        public async Task<PagedList<EduQuiz>> FindByParentAsync(ObjectId parentId, int pageNumber = 0, int pageSize = 10)
        {
            var query = _context.EduTestResult.Find(x => x.TestId == parentId);
            var total = await query.CountDocumentsAsync();

            var result = await query.Skip(pageNumber * pageSize).Limit(pageSize).ToListAsync();

            return new PagedList<EduQuiz>(result, pageNumber, pageSize, total);
        }
    }
}
