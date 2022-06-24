using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using User.Domain.Data.Interfaces;
using User.Domain.User;
using User.Infrastracture;

namespace User.Domain.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(
            UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserEntity>> FindAsync(int pageNumber = 0, int pageSize = 10)
        {
            return await _context.UserEntities.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<UserEntity>> FindAsync(Expression<Func<UserEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10)
        {
            return await _context.UserEntities.Where(query).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        //public async Task<KeycloakUserEntity> InsertOneAsync(KeycloakUserEntity userEntity)
        //{
        //    // save userEntity
        //    _context.UserEntities.Add(userEntity);
        //    await _context.SaveChangesAsync();

        //    return userEntity;
        //}

        public Task UpdateAsync(UserEntity userEntity)
        {
            _context.UserEntities.Update(userEntity);
            return _context.SaveChangesAsync();
        }

        //public Task DeleteAsync(KeycloakUserEntity userEntity)
        //{
        //    _context.UserEntities.Remove(userEntity);
        //    return _context.SaveChangesAsync();
        //}

        public async Task<UserEntity> FindByIdAsync(Guid id)
        {
            return await _context.UserEntities.FirstAsync(x=>x.Id == id.ToString());
        }


        public async Task<UserEntity> FindByUsernameAsync(string name)
        {
            return await _context.UserEntities.FirstOrDefaultAsync(x => x.Username == name);
        }

        public async Task<IEnumerable<UserAttribute>> GetAttributesAsync(Guid userId)
        {
            var user = await _context.UserEntities.Include(x => x.UserAttributes).FirstAsync(x => x.Id == userId.ToString());
            if (user == null)
            {
                return null;
            }
            return user.UserAttributes;
        }
    }
}