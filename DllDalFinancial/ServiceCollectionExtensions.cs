using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DllDalFinancial;
using DllEntityLayer;

namespace OtherDalAssembly
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDalAssemblyServices(this IServiceCollection services)
        {
            services.AddScoped<RedisAbstractGenericRepository<Ticker>, TickerRedisRepository>();
            services.AddScoped<SqlAbstractGenericRepository<Ticker>, TickerSqlRepository>();

            var configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build();            

            // Registrar servicios específicos de este ensamblado
            services.AddDbContext<MyDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        }
    }
}