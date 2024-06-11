using DllEntityLayer;

namespace DllDalFinancial;

public class TickerRedisRepository : RedisCrudRepository<Ticker>
{   
   public TickerRedisRepository(string connectionString) : base(connectionString)
    {
    }   
}