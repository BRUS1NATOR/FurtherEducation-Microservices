using System.Linq.Expressions;

namespace Profile.Domain.Diary.Repository
{
    public interface ITeacherDiaryRepository
    {
        Task<IEnumerable<TeacherDiaryEntity>> FindAsync(int pageNumber = 0, int pageSize = 10);
        Task<TeacherDiaryEntity?> FindOneAsync(Expression<Func<TeacherDiaryEntity, bool>> query);

        Task<IEnumerable<TeacherDiaryEntity>> FindAsync(Expression<Func<TeacherDiaryEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10);

        Task<TeacherDiaryEntity> CreateAsync(TeacherDiaryEntity diary);
        Task UpdateAsync(TeacherDiaryEntity diary);
    }
}