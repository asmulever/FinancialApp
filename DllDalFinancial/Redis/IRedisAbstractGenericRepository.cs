using System.Collections.Generic;
using System.Threading.Tasks;
using DllEntityLayer;

namespace DllDalFinancial
{
    public interface IRedisAbstractGenericRepository<T> where T : class, IEntityBase 
    {
        //Task<int> CreateAsync(T entity);
        //Task<T?> GetAsync(long? id);  // Allow nullable return type
        //Task<IEnumerable<T>> GetAllAsync();
        //Task UpdateAsync(long id, T entity);  // Ensure identifier type consistency
        //Task DeleteAsync(long id);  // Ensure identifier type consistency
    }
}
