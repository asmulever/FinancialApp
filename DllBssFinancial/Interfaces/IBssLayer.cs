namespace DllBssFinancial;
using DllDalFinancial;
using DllEntityLayer;

public interface IBssLayer
{ 
    IEntityBase get(string TktName);    
    bool set(IEntityBase tkt);
    bool del(IEntityBase tkt);
    bool upd(IEntityBase tkt); 

}