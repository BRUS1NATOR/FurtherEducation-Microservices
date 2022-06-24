using Education.Application.Data.Context;
using Education.Domain.Announcement;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Education.Application.Data.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ICourseContext _context;

        public AnnouncementRepository(ICourseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //CREATE
        public async Task<bool> Create(EduAnnouncement announcement)
        {
            await _context.Announcements.InsertOneAsync(announcement);

            var nameIndex = new CreateIndexModel<EduAnnouncement>(Builders<EduAnnouncement>.IndexKeys.Text(x => x.Name));

            _context.Announcements.Indexes.CreateOne(nameIndex);
            return await Task.FromResult(true);
        }

        //READ
        public async Task<EduAnnouncement> FindAsync(ObjectId id)
        {
            var filter = Builders<EduAnnouncement>.Filter.Eq(z => z.Id, id);

            return await _context.Announcements.Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<EduAnnouncement>> FindByParentAsync(ObjectId parentId)
        {
            var filter = Builders<EduAnnouncement>.Filter.Eq(z => z.Id, parentId);

            return await _context.Announcements.Find(filter).ToListAsync();
        }

        //UPDATE
        public async Task<bool> UpdateAsync(EduAnnouncement announcement)
        {
            var old = await FindAsync(announcement.Id);
            if (old == null)
            {
                return await Task.FromResult(false);
            }

            old.Name = announcement.Name;
            old.Description = announcement.Description;
            old.Content = announcement.Content;
            old.Order = announcement.Order;

            await WriteToDatabase(old);
            return await Task.FromResult(true);
        }

        public async Task<bool> WriteToDatabase(EduAnnouncement announcement)
        {
            var updateResult = await _context
                   .Announcements
                   .ReplaceOneAsync(g => g.Id == announcement.Id, announcement);

            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        //DELETE
        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var filter = Builders<EduAnnouncement>.Filter.Eq(z => z.Id, id);

            var deleteResult = await _context.Announcements.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
              && deleteResult.DeletedCount > 0;
        }
    }
}
