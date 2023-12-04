using ApiService.Api.ExceptionHandler;
using ApiService.Application;
using ApiService.Persistence;
using Ardalis.RouteAndBodyModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace ApiService.Api;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder app)
    {
        app.Services.Configure<RouteOptions>(options =>
            options.ConstraintMap.Add("List_ints", typeof(List<int>)));
        app.Services.AddApplicationService();
        app.Services.AddPersistenceServices(app.Configuration);
        

        app.Services.AddControllers(options =>
        {
            options.UseNamespaceRouteToken();
            options.ModelBinderProviders.InsertRouteAndBodyBinding();
        })
        .AddNewtonsoftJson();

        AddSwagger(app.Services);

        return app.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiService API");
            });
        }

        app.UseHttpsRedirection();
       // app.UseAuthentication();

        app.UseCustomExceptionHandler();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(app => app.MapControllers());

        return app;
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ServiceApi API"
            });

        });
    }

        public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var context = scope.ServiceProvider.GetService<ApiServiceDbContext>();
            if (context != null)
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

}
