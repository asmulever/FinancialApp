using StackExchange.Redis;
using System.Text.Json;

using DllDalFinancial;
using DllEntityLayer;

namespace DllDalFinancial;

public class TickerSqlRepository: SqlAbstractGenericRepository<Ticker>, ISqlAbstractGenericRepository<Ticker>
{
  public TickerSqlRepository(string connectionString) : base(connectionString)
    {
    }  
}
