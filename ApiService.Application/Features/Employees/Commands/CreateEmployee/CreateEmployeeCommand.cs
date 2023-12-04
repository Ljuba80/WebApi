using ApiService.Domain.Entities;
using MediatR;

namespace ApiService.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<CreateEmployeeCommandResponse>
{
    public string Email { get; set; } = string.Empty;
    public Title Title { get; set; }
    public ICollection<int> Companies { get; set; } = new List<int>();
}
