using Microsoft.EntityFrameworkCore;
using Profile.Domain.Diary;
using Profile.Domain.Diary.Repository;
using Profile.Infrastracture.Context;
using System.Linq.Expressions;

namespace Profile.Infrastracture.Diary
{
    public class TeacherDiaryRepository : ITeacherDiaryRepository
    {
        private readonly ProfileContext _context;

        public TeacherDiaryRepository(
            ProfileContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherDiaryEntity>> FindAsync(int pageNumber = 0, int pageSize = 10)
        {
            return await _context.TeacherDiaryEntities.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<TeacherDiaryEntity?> FindOneAsync(Expression<Func<TeacherDiaryEntity, bool>> query)
        {
            return await _context.TeacherDiaryEntities.Where(query).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TeacherDiaryEntity>> FindAsync(Expression<Func<TeacherDiaryEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10)
        {
            return await _context.TeacherDiaryEntities.Where(query).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<TeacherDiaryEntity> CreateAsync(TeacherDiaryEntity profile)
        {
            var entry = await _context.TeacherDiaryEntities.AddAsync(profile);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task UpdateAsync(TeacherDiaryEntity profile)
        {
            _context.TeacherDiaryEntities.Update(profile);
            await _context.SaveChangesAsync();
        }

    }
}