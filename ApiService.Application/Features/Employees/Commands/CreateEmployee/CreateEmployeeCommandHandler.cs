using ApiService.Application.Contracts.Persistence;
using ApiService.Application.Exceptions;
using ApiService.Domain.Entities;
using AutoMapper;
using MediatR;

namespace ApiService.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, CreateEmployeeCommandResponse>
{
    private readonly IMapper _mapper;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IAsyncRepository<SystemLog> _systemLogRepository;
    public CreateEmployeeCommandHandler(IMapper mapper, IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IAsyncRepository<SystemLog> systemLogRepository)
    {
        _mapper = mapper;
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
        _systemLogRepository = systemLogRepository;
    }
    public async Task<CreateEmployeeCommandResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateEmployeeCommandValidator(_employeeRepository, _companyRepository, _mapper);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Count > 0)
        {
            throw (new ValidationException(validationResult));
        }

        var createEmployeeCommandResponse = new CreateEmployeeCommandResponse();

        var @employee = _mapper.Map<Employee>(request);
        List<Company> companies = new List<Company>();
        foreach(int companyId in request.Companies)
        {
            Company? company = await _companyRepository.GetByIdAsync(companyId);
            companies.Add(company);
        }
        @employee.Companies = companies;
        DateTime now = DateTime.UtcNow;
        @employee.CreatedAt = now;
        @employee = await _employeeRepository.AddAsync(@employee);
        createEmployeeCommandResponse.EmployeeDto = _mapper.Map<EmployeeDto>(@employee);

        var @systemLog = new SystemLog
        {
            Comment = $"New Employee {employee.Email} created.",
            CreatedAt = now,
            Event = Event.Create,
            ResourceType = ResourceType.Employee,
            ResourceId = @employee.Id
        };

        await _systemLogRepository.AddAsync(@systemLog);

        return createEmployeeCommandResponse;
    }
}
