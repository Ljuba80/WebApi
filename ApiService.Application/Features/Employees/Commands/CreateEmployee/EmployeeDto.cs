using ApiService.Domain.Entities;

namespace ApiService.Application.Features.Employees.Commands.CreateEmployee;

public class EmployeeDto
{
    public string Email { get; set; } = string.Empty;
    public Title Title { get; set; }
}
