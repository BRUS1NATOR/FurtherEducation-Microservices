using FurtherEducation.Common.Data.Models;
using MongoDB.Bson;

namespace Education.Application.Data.Repositories
{
    public interface IRepositoty<TEntity> where TEntity : IDocument
    {
        //C
        Task<bool> Create(TEntity entity);
        Task<bool> WriteToDatabase(TEntity entity);
        //R
        Task<TEntity?> FindAsync(ObjectId id);
        //U
        Task<bool> UpdateAsync(TEntity entity);
        //D
        Task<bool> DeleteAsync(ObjectId id);
    }
}
