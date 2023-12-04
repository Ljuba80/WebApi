using ApiService.Application.Features.Companies.Commands.CreateCompany;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiService.Api.Endpoints.Compaines;

public class CreateCompanyEndpoint : EndpointBaseAsync
    .WithRequest<CreateCompanyCommand>
    .WithActionResult
{
    private readonly IMediator _mediator;
    public CreateCompanyEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("api/[namespace]")]
    public override async Task<ActionResult> HandleAsync([FromBody] CreateCompanyCommand createCompanyCommand,
    CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(createCompanyCommand, cancellationToken);
        return Ok(response);
    }
}
