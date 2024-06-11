using System.Data;
using DllEntityLayer;

namespace DllDalFinancial;

public class TickerSqlRepository: SqlAbstractGenericRepository<Ticker>, ISqlAbstractGenericRepository<Ticker>
{
  public TickerSqlRepository(IDbConnection dbConnection) : base(dbConnection)
    {
    }  
}
