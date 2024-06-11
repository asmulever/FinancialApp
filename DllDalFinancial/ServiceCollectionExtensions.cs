using Microsoft.Extensions.DependencyInjection;

using DllDalFinancial;
using DllEntityLayer;

namespace OtherDalAssembly;

    public static class ServiceCollectionExtensions
    {
        public static void AddDalAssemblyServices(this IServiceCollection services)
        {
            services.AddScoped<RedisCrudRepository<Ticker>, TickerRedisRepository>();
            services.AddScoped<SqlCrudRepository<Ticker>, TickerSqlRepository>();
        }
    }
