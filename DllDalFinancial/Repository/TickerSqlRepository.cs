using StackExchange.Redis;
using System.Text.Json;

using DllDalFinancial;
using DllEntityLayer;

namespace DllDalFinancial;

public class TickerSqlRepository
{
  public TickerSqlRepository(string connectionString) : base(connectionString)
    {
    }  
}
