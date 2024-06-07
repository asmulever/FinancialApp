using System.Collections.Generic;
using System.Threading.Tasks;
using DllEntityLayer;

namespace DllDalFinancial
{
    public interface ISqlAbstractGenericRepository<T> where T : class, IEntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<int> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> CreateTableIfNotExists();
    }
}

