using DllEntityLayer;

namespace DllDalFinancial;

public class TickerRedisRepository : RedisAbstractGenericRepository<Ticker>, IRedisAbstractGenericRepository<Ticker>
{   
   public TickerRedisRepository(string connectionString) : base(connectionString)
    {
    }  
 
}