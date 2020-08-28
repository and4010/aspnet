using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Repository.Interface
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(IDbTransaction transaction = null);
        Task DeleteRowAsync(long id, IDbTransaction transaction = null);
        Task<T> GetAsync(long id, IDbTransaction transaction = null);
        Task<int> SaveRangeAsync(IEnumerable<T> list, IDbTransaction transaction = null);
        Task UpdateAsync(T t, IDbTransaction transaction = null);
        Task<long?> InsertAsync(T t, IDbTransaction transaction = null);
        Task<int> CountAsync(IDbTransaction transaction = null);
        Task TruncateAsync(IDbTransaction transaction = null);
    }
}
