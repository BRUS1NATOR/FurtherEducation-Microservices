using Microsoft.EntityFrameworkCore;
using Profile.Domain.Profile;
using Profile.Infrastracture.Context;
using System.Linq.Expressions;
using User.Domain.Profile.Repository;

namespace Profile.Infrastracture.Profile
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ProfileContext _context;

        public ProfileRepository(
            ProfileContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProfileEntity>> FindAsync(int pageNumber = 0, int pageSize = 10)
        {
            return await _context.ProfileEntities.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<IEnumerable<ProfileEntity>> FindAsync(Expression<Func<ProfileEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10)
        {
            return await _context.ProfileEntities.Where(query).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<ProfileEntity?> FindByIdAsync(Guid id)
        {
            return await _context.ProfileEntities.Include(x=>x.StudentDiaries).Include(x=>x.TeacherDiaries).FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<ProfileEntity> CreateAsync(ProfileEntity profile)
        {
            var entry = await _context.ProfileEntities.AddAsync(profile);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task UpdateAsync(ProfileEntity profile)
        {
            _context.ProfileEntities.Update(profile);
            await _context.SaveChangesAsync();
        }
    }
}