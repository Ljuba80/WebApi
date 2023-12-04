using ApiService.Application.Contracts.Persistence;
using ApiService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiService.Persistence.Repositories;

public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
{
    public CompanyRepository(ApiServiceDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> CompanyExistsAsync(string companyName)
    {
        Company? company = await _dbContext.Companies.FirstOrDefaultAsync(p=>p.Name == companyName);
        return company != null;
    }
    public async Task<bool> TitleExistsAsync(int companyId, Title title)
    {
        Company? company = await _dbContext.Companies.FindAsync(companyId);
        if(company == null || company.Employees == null)
        {
            return false;
        }

        return company.Employees.Any(p => p.Title == title);
    }
}
