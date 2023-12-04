using ApiService.Application.Responses;

namespace ApiService.Application.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandResponse : BaseResponse
{
    public CreateCompanyCommandResponse() : base()
    {
    }
    public CompanyDto CompanyDto { get; set; } = default!;
}
