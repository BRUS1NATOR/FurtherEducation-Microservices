using Education.Application.Data.Context;
using Education.Domain.EduModules;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Education.Application.Data.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ICourseContext _context;

        public ModuleRepository(ICourseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //CREATE
        public async Task<bool> Create(EduModule module)
        {
            await _context.Modules.InsertOneAsync(module);

            var nameIndex = new CreateIndexModel<EduModule>(Builders<EduModule>.IndexKeys.Text(x => x.Name));

            _context.Modules.Indexes.CreateOne(nameIndex);

            return await Task.FromResult(true);
        }

        //READ
        public async Task<EduModuleDetailed> FindDetailedAsync(ObjectId id)
        {
            var filter = Builders<EduModule>.Filter.Eq(x => x.Id, id);
            var q = _context.Modules.Aggregate().Match(filter)
                .Lookup(_context.Announcements, c => c.Id, a => a.ModuleId, (EduModuleDetailed e) => e.Announcements)
                .Lookup(_context.EduTasks, c => c.Id, task => task.ModuleId, (EduModuleDetailed e) => e.Tasks)
                .Lookup(_context.EduTests, c => c.Id, test => test.ModuleId, (EduModuleDetailed e) => e.Tests)
                .As<EduModuleDetailed>();

            return await q.FirstOrDefaultAsync();
            //LINQ
            //var query = from p in _context.Courses.AsQueryable().Where(x=>x.Id == id)
            //            join m in _context.Modules.AsQueryable() on p.Id equals m.CourseId into moduleCol
            //            join a in _context.Announcements.AsQueryable() on p.Id equals a.CourseId into announceCol
            //            select new CourseDetailed()
            //            {
            //                ContentImage = p.ContentImage,
            //                Description = p.Description,
            //                Hours = p.Hours,
            //                Id = p.Id,
            //                Name = p.Name,
            //                Professor = p.Professor,
            //                Speciality = p.Speciality,
            //                Students = p.Students,
            //                TotalMembers = p.TotalMembers,
            //                Modules = moduleCol,
            //                Announcements = announceCol
            //            };
        }
        public async Task<EduModule> FindAsync(ObjectId id)
        {
            var filter = Builders<EduModule>.Filter
                 .Eq(z => z.Id, id);

            return await _context.Modules.Aggregate()
                .Match(filter)
                .FirstOrDefaultAsync(); // or .ToList() to return multiple
        }

        //UPDATE
        public async Task<bool> UpdateAsync(EduModule module)
        {
            var old = await FindAsync(module.Id);
            if (old == null)
            {
                return await Task.FromResult(false);
            }

            old.Name = module.Name;
            old.Description = module.Description;
            old.Content = module.Content;
            old.Order = module.Order;

            await WriteToDatabase(old);

            return await Task.FromResult(true);
        }

        public async Task<bool> WriteToDatabase(EduModule module)
        {
            var updateResult = await _context
                .Modules
                .ReplaceOneAsync(g => g.Id == module.Id, module);

            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        //DELETE
        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var filter = Builders<EduModule>.Filter.Eq(x => x.Id, id);
            var deleteResult = await _context.Modules.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
              && deleteResult.DeletedCount > 0;
        }
    }
}
