using System.Runtime.CompilerServices;
using DllDalFinancial.Redis;
using DllEntityLayer;

namespace DllBssFinancial;
public class BssLayer : IBssLayer
{
    private IRedisAbstractGenericRepository<Ticker> _DataRedisLayer ;

    //constructor
    public BssLayer(IRedisAbstractGenericRepository<Ticker> DataRedisLayer)
    {
        _DataRedisLayer = DataRedisLayer;
    }

    public IEntityBase get(IEntityBase Tkt)
    {       
        return _DataRedisLayer.CreateAsync(Tkt);
    }

    public bool set(IEntityBase tkt)
    {
        return true;
    }

    public bool del(IEntityBase tkt)
    {
        return true;
    }

    public bool upd(IEntityBase tkt)
    {
        return true;
    }

}
