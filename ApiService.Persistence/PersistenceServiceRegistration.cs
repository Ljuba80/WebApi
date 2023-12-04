using ApiService.Application.Contracts.Persistence;
using ApiService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiService.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("ApiServiceConnectionString");
        services.AddDbContext<ApiServiceDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).UseLazyLoadingProxies());

        services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();

        return services;
    }
}
