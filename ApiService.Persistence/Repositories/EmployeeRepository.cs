using ApiService.Application.Contracts.Persistence;
using ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiService.Persistence.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(ApiServiceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> EmployeeExistsAsync(string email)
    {
        Employee? employee = await _dbContext.Employees.FirstOrDefaultAsync(p => p.Email == email);
        return employee != null;
    }
}
