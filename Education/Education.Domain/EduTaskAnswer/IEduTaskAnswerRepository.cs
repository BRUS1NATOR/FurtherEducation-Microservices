using FurtherEducation.Common.Models;
using Education.Domain.EduTaskAnswers;
using MongoDB.Bson;

namespace Education.Application.Data.Repositories
{
    public interface IEduTaskAnswerRepository : IRepositoty<EduTaskAnswer>
    {
        Task<PagedList<EduTaskAnswer>> FindByParentAsync(ObjectId parentId, int page, int pageSize);
        Task<EduTaskAnswer> FindByParentAndUserAsync(ObjectId parentId, Guid userId);
    }
}
