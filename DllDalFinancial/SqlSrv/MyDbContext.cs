using Microsoft.EntityFrameworkCore;
using DllEntityLayer;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<Ticker> Tickers { get; set; }
    // Agrega otros DbSet para otras entidades según sea necesario

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configuraciones adicionales del modelo, si las hay
    }
}
