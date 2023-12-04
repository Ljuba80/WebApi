using ApiService.Domain.Entities;

namespace ApiService.Application.Contracts.Persistence;

public interface IEmployeeRepository : IAsyncRepository<Employee>
{
    Task<bool> EmployeeExistsAsync(string email);
}
