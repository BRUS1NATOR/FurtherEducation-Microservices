using Education.Domain.Announcement;
using MongoDB.Bson;

namespace Education.Application.Data.Repositories
{
    public interface IAnnouncementRepository : IRepositoty<EduAnnouncement>
    {
        Task<IList<EduAnnouncement>> FindByParentAsync(ObjectId parentId);
    }
}
