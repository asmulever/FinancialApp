using StackExchange.Redis;
using System.Text.Json;

using DllDalFinancial;
using DllDalFinancial.Redis;
using DllDalFinancial.SqlSrv;
using DllEntityLayer;

namespace DllDalFinancial;

public class TickerRedisRepository : RedisAbstractGenericRepository<Ticker>, IRedisAbstractGenericRepository<Ticker>
{   
   public TickerRedisRepository(string connectionString) : base(connectionString)
    {
    }  
 
}

public class TickerSqlRepository : SqlAbstractGenericRepository<Ticker>, ISqlAbstractGenericRepository<Ticker>
{   
   public TickerSqlRepository(MyDbContext connectionString) : base(connectionString)
    {
    }  
 
}