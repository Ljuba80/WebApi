using ApiService.Application.Contracts.Persistence;
using ApiService.Application.Exceptions;
using ApiService.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ApiService.Application.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyCommandResponse>
{
    private readonly IMapper _mapper;
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAsyncRepository<SystemLog> _systemLogRepository;
    public CreateCompanyCommandHandler(IMapper mapper, ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IAsyncRepository<SystemLog> systemLogRepository)
    {
        _mapper = mapper;
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
        _systemLogRepository = systemLogRepository;
    }

    public async Task<CreateCompanyCommandResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateCompanyCommandValidator(_companyRepository, _employeeRepository, _mapper);
        var validationResult = await validator.ValidateAsync(request);
        var createCompanyCommandResponse = new CreateCompanyCommandResponse();

        if (validationResult.Errors.Count > 0)
        {
            throw (new ValidationException(validationResult));
        }

        var @company = _mapper.Map<Company>(request);
        DateTime now = DateTime.UtcNow;
        List<Employee> employees = @company.Employees.ToList();
        employees.ForEach(p => p.CreatedAt = now);
        foreach(int id in request.EmployeesIds)
        {
            Employee employee = await _employeeRepository.GetByIdAsync(id);
            employees.Add(employee);
        }
        @company.Employees = employees;
        @company.CreatedAt = now;
        @company = await _companyRepository.AddAsync(@company);
        
        createCompanyCommandResponse.CompanyDto = _mapper.Map<CompanyDto>(@company);

        var @systemLog = new SystemLog
        {
            Comment = $"New company {@company.Name} created.",
            CreatedAt = now,
            Event = Event.Create,
            ResourceType = ResourceType.Company,
            ResourceId = @company.Id
        };

        await _systemLogRepository.AddAsync(@systemLog);
      
        return createCompanyCommandResponse;
    }
}
