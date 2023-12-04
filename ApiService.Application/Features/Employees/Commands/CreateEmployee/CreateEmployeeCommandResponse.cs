using ApiService.Application.Responses;

namespace ApiService.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandResponse : BaseResponse
{
    public CreateEmployeeCommandResponse() : base()
    {
    }
    public EmployeeDto EmployeeDto { get; set; } = default!;
}
