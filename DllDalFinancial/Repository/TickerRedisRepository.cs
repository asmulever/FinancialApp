using StackExchange.Redis;
using System.Text.Json;

using DllDalFinancial;
using DllEntityLayer;

namespace DllDalFinancial;

public class TickerRedisRepository : RedisAbstractGenericRepository<Ticker>, IRedisAbstractGenericRepository<Ticker>
{   
   public TickerRedisRepository(string connectionString) : base(connectionString)
    {
    }  
 
}