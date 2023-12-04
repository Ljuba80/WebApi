using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ApiService.Application;

public static class AppServiceRegistration
{
    public static IServiceCollection AddApplicationService(this IServiceCollection Services)
    {
        Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        Services.AddAutoMapper(Assembly.GetExecutingAssembly());
      
        return Services;
    }
}
