using System.Collections.Generic;
using System.Threading.Tasks;
using DllEntityLayer;

namespace DllDalFinancial.Interfaces;

    public interface IGenericCrudRepository<T> where T : class, IEntityBase
    {
        Task<T?> GetByID(int id);  // Allow nullable return type
        Task<IEnumerable<T>> GetAll();
        Task<int> Ins(T entity);        
        Task<bool> Upd (int id, T entity);  // Ensure identifier type consistency
        Task<bool> Del(int id);  // Ensure identifier type consistency
    }
