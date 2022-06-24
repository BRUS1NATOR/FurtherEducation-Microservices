using Education.Application.Data.Context;
using Education.Domain.EduModules;
using Education.Domain.EduTests;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Education.Application.Data.Repositories
{
    public class EduTestRepository : IEduTestRepository
    {
        private readonly ICourseContext _context;
        private readonly ILogger _logger;

        public EduTestRepository(ICourseContext context, ILogger<EduTestRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        //CREATE
        public async Task<bool> Create(EduTest test)
        {
            if (test.StudentAnswers is null)
            {
                test.StudentAnswers = new List<ObjectId>();
            }
            test.AnswerType = Domain.Enum.AnswerType.Text;
            await _context.EduTests.InsertOneAsync(test);

            var nameIndex = new CreateIndexModel<EduModule>(Builders<EduModule>.IndexKeys.Text(x => x.Name));

            _context.Modules.Indexes.CreateOne(nameIndex);
            return await Task.FromResult(true);
        }

        //READ
        public async Task<EduTest> FindAsync(ObjectId id)
        {
            var filter = Builders<EduTest>.Filter.Eq(z => z.Id, id);

            return await _context.EduTests
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<EduModule> FindModuleByTestAsync(ObjectId id)
        {
            var filter = Builders<EduModule>.Filter.Eq(z => z.Id, id);

            return await _context.Modules.Aggregate().Match(filter).FirstOrDefaultAsync();
        }

        //UPDATE
        public async Task<bool> UpdateAsync(EduTest entity)
        {
            var old = await FindAsync(entity.Id);
            if (old == null)
            {
                return await Task.FromResult(false);
            }

            old.Name = entity.Name;
            old.Description = entity.Description;
            old.Content = entity.Content;
            old.ExpirationDate = entity.ExpirationDate;
            old.TestSettings = entity.TestSettings;
            old.Order = entity.Order;

            await WriteToDatabase(old);

            return await Task.FromResult(true);
        }

        public async Task<bool> WriteToDatabase(EduTest test)
        {
            var updateResult = await _context
              .EduTests
              .ReplaceOneAsync(g => g.Id == test.Id, test);

            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }
        //DELETE
        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var filter = Builders<EduTest>.Filter.Eq(x => x.Id, id);

            var deleteResult = await _context.EduTests.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
              && deleteResult.DeletedCount > 0;
        }
    }
}
