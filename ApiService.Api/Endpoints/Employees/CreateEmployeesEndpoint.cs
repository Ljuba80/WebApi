using Ardalis.ApiEndpoints;
using ApiService.Application.Features.Employees.Commands.CreateEmployee;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Api.Endpoints.Employees;

public class CreateEmployeesEndpoint : EndpointBaseAsync
    .WithRequest<CreateEmployeeCommand>
    .WithActionResult
{
    private readonly IMediator _mediator;
    public CreateEmployeesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("api/[namespace]")]
    public override async Task<ActionResult> HandleAsync([FromBody] CreateEmployeeCommand createEmployeeCommand,
    CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(createEmployeeCommand);
        return Ok(response);
    }
}
