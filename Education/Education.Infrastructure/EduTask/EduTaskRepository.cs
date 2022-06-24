using Education.Application.Data.Context;
using Education.Domain.EduModules;
using Education.Domain.EduTasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Education.Application.Data.Repositories
{
    public class EduTaskRepository : IEduTaskRepository
    {
        private readonly ICourseContext _context;

        public EduTaskRepository(ICourseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //CREATE
        public async Task<bool> Create(EduTask task)
        {
            if (task.StudentAnswers is null)
            {
                task.StudentAnswers = new List<ObjectId>();
            }

            await _context.EduTasks.InsertOneAsync(task);

            var nameIndex = new CreateIndexModel<EduModule>(Builders<EduModule>.IndexKeys.Text(x => x.Name));

            _context.Modules.Indexes.CreateOne(nameIndex);
            return await Task.FromResult(true);
        }

        //READ
        public async Task<EduTask> FindAsync(ObjectId id)
        {
            var filter = Builders<EduTask>.Filter.Eq(z => z.Id, id);

            return await _context.EduTasks
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        //UPDATE
        public async Task<bool> UpdateAsync(EduTask entity)
        {
            var old = await FindAsync(entity.Id);
            if (old == null)
            {
                return await Task.FromResult(false);
            }

            old.Name = entity.Name;
            old.Description = entity.Description;
            old.Content = entity.Content;
            old.AnswerType = entity.AnswerType;
            old.ExpirationDate = entity.ExpirationDate;
            old.Order = entity.Order;

            await WriteToDatabase(old);

            return await Task.FromResult(true);
        }

        public async Task<bool> WriteToDatabase(EduTask task)
        {
            var updateResult = await _context
                  .EduTasks
                  .ReplaceOneAsync(g => g.Id == task.Id, task);

            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        //DELETE
        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var filter = Builders<EduTask>.Filter.Eq(x => x.Id, id);

            var deleteResult = await _context.EduTasks.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
              && deleteResult.DeletedCount > 0;
        }
    }
}
