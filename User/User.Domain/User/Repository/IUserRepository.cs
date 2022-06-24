using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using User.Domain.User;

namespace User.Domain.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> FindAsync(int pageNumber = 0, int pageSize = 10);
        Task<IEnumerable<UserEntity>> FindAsync(Expression<Func<UserEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10);

        Task<UserEntity> FindByUsernameAsync(string name);
        Task<UserEntity> FindByIdAsync(Guid id);

        Task<IEnumerable<UserAttribute>> GetAttributesAsync(Guid userId);
        Task UpdateAsync(UserEntity user);
    }
}