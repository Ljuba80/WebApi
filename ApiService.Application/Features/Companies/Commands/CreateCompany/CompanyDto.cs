namespace ApiService.Application.Features.Companies.Commands.CreateCompany;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
