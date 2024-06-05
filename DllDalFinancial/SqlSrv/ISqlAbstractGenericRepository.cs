using DllEntityLayer;

namespace DllDalFinancial;

public interface ISqlAbstractGenericRepository<T> where T : class, IEntityBase
{
        Task<int> CreateAsync(T entity);
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
}
