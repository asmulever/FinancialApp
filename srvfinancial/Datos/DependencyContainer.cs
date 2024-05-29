using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OtherBssAssembly;

namespace srvfinancial;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        #region Services Injection
        //services.AddScoped<IUserService, UserService>();
        #endregion

        #region Repositories Injection
        //services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddBssAssemblyServices();

        #endregion
    }
}