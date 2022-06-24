using Microsoft.EntityFrameworkCore;
using Profile.Domain.Diary;
using Profile.Domain.Diary.Repository;
using Profile.Infrastracture.Context;
using System.Linq.Expressions;

namespace Profile.Infrastracture.Diary
{
    public class StudentDiaryRepository : IStudentDiaryRepository
    {
        private readonly ProfileContext _context;

        public StudentDiaryRepository(
            ProfileContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentDiaryEntity>> FindAsync(int pageNumber = 0, int pageSize = 10)
        {
            return await _context.StudentDiaryEntities.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<StudentDiaryEntity?> FindOneAsync(Expression<Func<StudentDiaryEntity, bool>> query)
        {
            return await _context.StudentDiaryEntities.Include(x => x.Records).Where(query).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<StudentDiaryEntity>> FindAsync(Expression<Func<StudentDiaryEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10)
        {
            return await _context.StudentDiaryEntities.Where(query).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<StudentDiaryEntity> CreateAsync(StudentDiaryEntity profile)
        {
            var entry = await _context.StudentDiaryEntities.AddAsync(profile);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task UpdateAsync(StudentDiaryEntity profile)
        {
            _context.StudentDiaryEntities.Update(profile);
            await _context.SaveChangesAsync();
        }

    }
}