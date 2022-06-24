using System.Linq.Expressions;

namespace Profile.Domain.Diary.Repository
{
    public interface IDiaryRecordRepository
    {
        Task<IEnumerable<DiaryRecordEntity>> FindAsync(int pageNumber = 0, int pageSize = 10);
        Task<DiaryRecordEntity?> FindOneAsync(Expression<Func<DiaryRecordEntity, bool>> query);

        Task<IEnumerable<DiaryRecordEntity>> FindAsync(Expression<Func<DiaryRecordEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10);

        Task<DiaryRecordEntity> CreateAsync(DiaryRecordEntity diary);
        Task UpdateAsync(DiaryRecordEntity diary);
    }
}