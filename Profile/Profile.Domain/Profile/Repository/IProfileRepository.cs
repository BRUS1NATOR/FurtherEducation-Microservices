using Profile.Domain.Profile;
using System.Linq.Expressions;

namespace User.Domain.Profile.Repository
{
    public interface IProfileRepository
    {
        Task<IEnumerable<ProfileEntity>> FindAsync(int pageNumber = 0, int pageSize = 10);
        Task<IEnumerable<ProfileEntity>> FindAsync(Expression<Func<ProfileEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10);

        Task<ProfileEntity> FindByIdAsync(Guid id);

        Task<ProfileEntity> CreateAsync(ProfileEntity profile);
        Task UpdateAsync(ProfileEntity profile);
    }
}