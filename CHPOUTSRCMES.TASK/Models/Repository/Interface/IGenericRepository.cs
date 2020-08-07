using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Models.Repository.Interface
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteRowAsync(long id);
        Task<T> GetAsync(long id);
        Task<int> SaveRangeAsync(IEnumerable<T> list);
        Task UpdateAsync(T t);
        Task<long?> InsertAsync(T t);
        Task<int> CountAsync();
        Task TruncateAsync();
    }
}
