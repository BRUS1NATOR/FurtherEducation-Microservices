using Education.Domain.EduQuizes;
using FurtherEducation.Common.Models;
using MongoDB.Bson;

namespace Education.Application.Data.Repositories
{
    public interface IEduQuizRepository : IRepositoty<EduQuiz>
    {
        Task<PagedList<EduQuiz>> FindByParentAsync(ObjectId parentId, int page, int pageSize);
        Task<EduQuiz> FindByParentAndUserAsync(ObjectId parentId, Guid userId);
    }
}
