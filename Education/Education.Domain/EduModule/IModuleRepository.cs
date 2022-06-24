using Education.Domain.EduModules;
using MongoDB.Bson;

namespace Education.Application.Data.Repositories
{
    public interface IModuleRepository : IRepositoty<EduModule>
    {
        Task<EduModuleDetailed> FindDetailedAsync(ObjectId id);
    }
}