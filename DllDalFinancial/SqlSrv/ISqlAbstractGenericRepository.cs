using System.Collections.Generic;
using System.Threading.Tasks;
using DllEntityLayer;

namespace DllDalFinancial
{
    public interface ISqlAbstractGenericRepository<T> where T : class, IEntityBase 
    {
        //Task<IEnumerable<T>> GetAllAsync();
        //Task<T> GetByIdAsync(long? id);
        //Task<T> InsertAsync<T>(T entity);
        //Task<bool> UpdateAsync(T entity);
        //Task<bool> DeleteAsync(long? id);
        //Task<bool> CreateTableIfNotExists();
    }
}

