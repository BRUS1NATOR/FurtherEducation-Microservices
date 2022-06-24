using Profile.Domain.Profile;
using System.Linq.Expressions;

namespace Profile.Domain.Diary.Repository
{
    public interface IStudentDiaryRepository
    {
        Task<IEnumerable<StudentDiaryEntity>> FindAsync(int pageNumber = 0, int pageSize = 10);
        Task<StudentDiaryEntity?> FindOneAsync(Expression<Func<StudentDiaryEntity, bool>> query);

        Task<IEnumerable<StudentDiaryEntity>> FindAsync(Expression<Func<StudentDiaryEntity, bool>> query, int pageNumber = 0,
            int pageSize = 10);

        Task<StudentDiaryEntity> CreateAsync(StudentDiaryEntity diary);
        Task UpdateAsync(StudentDiaryEntity diary);
    }
}