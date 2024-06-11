using System.Data;
using DllEntityLayer;

namespace DllDalFinancial;

public class TickerSqlRepository: SqlCrudRepository<Ticker>
{
  public TickerSqlRepository(IDbConnection dbConnection) : base(dbConnection)
    {
    }  
}
