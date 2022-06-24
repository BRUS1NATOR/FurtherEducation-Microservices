using Education.Domain.EduCourses;
using FurtherEducation.Common.Models;
using MongoDB.Bson;
using System.Linq.Expressions;


namespace Education.Application.Data.Repositories
{
    public interface ICourseRepository : IRepositoty<EduCourse>
    {
        Task<EduCourseDetailed> FindDetailedAsync(ObjectId id);
        Task<PagedList<EduCourse>> FindAsync(int pageNumber = 0, int pageSize = 20);
        Task<PagedList<EduCourse>> FindAsync(string query, int pageNumber = 0, int pageSize = 20);
        Task<PagedList<EduCourse>> FindAsync(Expression<Func<EduCourse, bool>> query, int pageNumber = 0,
            int pageSize = 20);

        Task<EduCourse?> FindByModuleAsync(ObjectId objectId);
        Task<EduCourse?> FindByAnnouncementAsync(ObjectId taskId);
        Task<EduCourse?> FindByTaskAsync(ObjectId taskId);
        Task<EduCourse?> FindByTestAsync(ObjectId testId);
    }
}