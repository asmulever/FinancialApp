
namespace DllEntityLayer;

/* public class Ticker : IEntityBase
{
    public int Id { get; set; }  = 0; 
    public DateTime Fecha { get; set; } = DateTime.Now;
    public string tickerName { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public int Categoria { get; set; }
} */

public class Ticker : IEntityBase
{
    public int Id { get; set; }  // Change from int to long
    public DateTime Fecha { get; set; }  = DateTime.Now;
    public string tickerName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Categoria { get; set; }
}