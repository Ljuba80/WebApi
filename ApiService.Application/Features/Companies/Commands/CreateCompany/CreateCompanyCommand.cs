using ApiService.Application.Features.Employees.Commands.CreateEmployee;
using MediatR;

namespace ApiService.Application.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommand : IRequest<CreateCompanyCommandResponse>
{
    public string Name { get; set; } = string.Empty;
    public ICollection<int> EmployeesIds { get; set; } = new List<int>();
    public ICollection<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
}
