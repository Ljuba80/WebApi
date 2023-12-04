using ApiService.Domain.Entities;

namespace ApiService.Application.Contracts.Persistence;

public interface ICompanyRepository : IAsyncRepository<Company>
{
    Task<bool> CompanyExistsAsync(string companyName);
    public Task<bool> TitleExistsAsync(int companyId, Title title);
}
