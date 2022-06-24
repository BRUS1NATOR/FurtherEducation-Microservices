using Education.Application.Data.Context;
using Education.Domain.EduTaskAnswers;
using FurtherEducation.Common.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Education.Application.Data.Repositories
{
    public class EduTaskAnswerRepository : IEduTaskAnswerRepository
    {
        private readonly ICourseContext _context;

        public EduTaskAnswerRepository(ICourseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //CREATE
        public async Task<bool> Create(EduTaskAnswer answer)
        {
            if (answer.Files is null)
            {
                answer.Files = new List<FileAnswer>();
            }

            await _context.EduTaskAnswers.InsertOneAsync(answer);

            return await Task.FromResult(true);
        }

        //READ
        public async Task<EduTaskAnswer> FindAsync(ObjectId id)
        {
            var filter = Builders<EduTaskAnswer>.Filter.Eq(z => z.Id, id);

            return await _context.EduTaskAnswers
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<EduTaskAnswer> FindByParentAndUserAsync(ObjectId parentId, Guid userId)
        {
            var filter = Builders<EduTaskAnswer>.Filter.Eq(z => z.TaskId, parentId)
                       & Builders<EduTaskAnswer>.Filter.Eq(z => z.UserId, userId);

            return await _context.EduTaskAnswers
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        //UPDATE
        public async Task<bool> UpdateAsync(EduTaskAnswer entity)
        {
            var old = await FindAsync(entity.Id);
            if (old == null)
            {
                return await Task.FromResult(false);
            }

            old.TextAnswer = entity.TextAnswer;
            old.Files = entity.Files;
            old.Score = entity.Score;

            await WriteToDatabase(old);

            return await Task.FromResult(true);
        }

        public async Task<bool> WriteToDatabase(EduTaskAnswer task)
        {
            var updateResult = await _context
                  .EduTaskAnswers
                  .ReplaceOneAsync(g => g.Id == task.Id, task);

            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        //DELETE
        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var filter = Builders<EduTaskAnswer>.Filter.Eq(x => x.Id, id);

            var deleteResult = await _context.EduTaskAnswers.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
              && deleteResult.DeletedCount > 0;
        }

        public async Task<PagedList<EduTaskAnswer>> FindByParentAsync(ObjectId parentId, int pageNumber = 0, int pageSize = 10)
        {
            var query = _context.EduTaskAnswers.Find(x => x.TaskId == parentId);
            var total = await query.CountDocumentsAsync();

            var result = await query.Skip(pageNumber * pageSize).Limit(pageSize).ToListAsync();

            return new PagedList<EduTaskAnswer>(result, pageNumber, pageSize, total);
        }
    }
}
