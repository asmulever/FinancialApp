using Microsoft.Extensions.DependencyInjection;
using OtherDalAssembly;
using DllBssFinancial;
using DllEntityLayer;

namespace OtherBssAssembly
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBssAssemblyServices(this IServiceCollection services)
        {
            services.AddScoped<IBssLayer, BssLayer>();
            services.AddDalAssemblyServices();
        }
    }
}