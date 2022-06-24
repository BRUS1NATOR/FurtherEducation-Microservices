using Microsoft.EntityFrameworkCore;
using Profile.Domain.Diary;
using Profile.Domain.Diary.Repository;
using Profile.Infrastracture.Context;
using System.Linq.Expressions;

namespace Profile.Infrastracture.Diary
{
    public class StudentDiaryRecordRepository : IDiaryRecordRepository
    {
        private readonly ProfileContext _context;

        public StudentDiaryRecordRepository(
            ProfileContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiaryRecordEntity>> FindAsync(int pageNumber = 0, int pageSize = 10)
        {
            return await _context.DiaryRecords.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<DiaryRecordEntity?> FindOneAsync(Expression<Func<DiaryRecordEntity, bool>> query)
        {
            return await _context.DiaryRecords.Where(query).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DiaryRecordEntity>> FindAsync(Expression<Func<DiaryRecordEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10)
        {
            return await _context.DiaryRecords.Where(query).Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<DiaryRecordEntity> CreateAsync(DiaryRecordEntity record)
        {
            var entry = await _context.DiaryRecords.AddAsync(record);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async Task UpdateAsync(DiaryRecordEntity record)
        {
            _context.DiaryRecords.Update(record);
            await _context.SaveChangesAsync();
        }

    }
}