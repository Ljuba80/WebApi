using ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiService.Persistence;

public class ApiServiceDbContext : DbContext
{
    public ApiServiceDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<SystemLog> SystemLogs => Set<SystemLog>();
     

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiServiceDbContext).Assembly);
    }


}
