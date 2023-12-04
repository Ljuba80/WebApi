using ApiService.Application.Contracts.Persistence;
using ApiService.Domain.Entities;
using AutoMapper;
using FluentValidation;
using System.Text;

namespace ApiService.Application.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository; 
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    private string companyName = string.Empty;
    private string errorMessage = string.Empty;
    public CreateCompanyCommandValidator(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
        _mapper = mapper;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .NotNull()
            .MaximumLength(50)
            .WithMessage("Company name  must not exceed 50 characters.")
            .MinimumLength(1)
            .WithMessage("Company name  must not be empty.");
        RuleFor(e => e)
            .MustAsync(CompanyNameUnique)
            .WithMessage($"Company with the same name ({companyName}) already exists.")

            .MustAsync(EmployeesUnique)
            .WithErrorCode($"Adding company ${companyName}")
            .WithMessage(x=>errorMessage)

            .MustAsync(EmployeesExists)
            .WithErrorCode($"Adding company ${companyName}")
            .WithMessage(x=>errorMessage)

            .MustAsync(EmployeeTitleUnique)
            .WithErrorCode($"Adding company ${companyName}")
            .WithMessage(x=>errorMessage);
    }

    private async Task<bool> CompanyNameUnique(CreateCompanyCommand e, CancellationToken token)
    {
        companyName = e.Name;
        return !(await _companyRepository.CompanyExistsAsync(e.Name));
    }
    private async Task<bool> EmployeeTitleUnique(CreateCompanyCommand e, CancellationToken token)
    {
        errorMessage = string.Empty;

        var employeeCandidates = new List<Employee>();
        //Validate existing employees if any
        if (e.EmployeesIds != null)
        {
            await Parallel.ForEachAsync(e.EmployeesIds, async (item, cancellationToken) =>
            {
                Employee? employee = await _employeeRepository.GetByIdAsync(item);

                if (employee != null)
                {
                    employeeCandidates.Add(employee);
                }
            });
        }

        //Validate new employees if any also

        if (e.Employees != null)
        {
            foreach (var @employeeDto in e.Employees)
            {
                employeeCandidates.Add(_mapper.Map<Employee>(@employeeDto));
            }
        }

        var invalidEmployeesIds = employeeCandidates.GroupBy(x => x.Title)
          .Where(g => g.Count() > 1)
          .ToDictionary(g => g.Key, g => g.Select(p => p.Id).ToList());

        if (invalidEmployeesIds.Count > 0)
        {
            var invalidEmployeesErrorMsg = new StringBuilder($"Titles not unigue for the company {e.Name}:");
            foreach (var item in invalidEmployeesIds)
            {
                var tempBuilder = new StringBuilder();
                invalidEmployeesErrorMsg.Append($"Title:{item.Key}/Ids:");
                item.Value.ForEach(p => { tempBuilder.Append(p); tempBuilder.Append(" "); });
                invalidEmployeesErrorMsg.Append(tempBuilder);
            }
            errorMessage = invalidEmployeesErrorMsg.ToString();
        }
        return invalidEmployeesIds.Count == 0;
    }

    private async Task<bool> EmployeesExists(CreateCompanyCommand e, CancellationToken token)
    {
        errorMessage = string.Empty;
        bool result = true;

        var invalidEmployeesErrorMsg = new StringBuilder("Non existent employees:");
        await Parallel.ForEachAsync(e.EmployeesIds, async (item, cancellationToken) =>
        {
            Employee? employee = await _employeeRepository.GetByIdAsync(item);
            if (employee == null)
            {
                result = false;
                invalidEmployeesErrorMsg.Append($"{item} ");
            }
        });

        errorMessage = invalidEmployeesErrorMsg.ToString();
        return result;
    }

    private async Task<bool> EmployeesUnique(CreateCompanyCommand e, CancellationToken token)
    {
        await Task.Yield();

        errorMessage = string.Empty;
        //check for duplicated ids passed
        List<int> duplicatedIds = new List<int>();
        List<string> duplicatedNames = new List<string>();

        if (e.EmployeesIds != null)
        {
            duplicatedIds = e.EmployeesIds.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();
        }
        if(e.Employees != null)
        {
            var employees = await _employeeRepository.GetAllAsync();
            if(employees != null) 
            {
                duplicatedNames = employees.Select(p => p.Email).Intersect(e.Employees.Select(p => p.Email)).ToList();
            }

        }
        var invalidEmployeesErrorMsg = new StringBuilder();

        if (duplicatedIds.Count > 0)
        {
            invalidEmployeesErrorMsg.Append($"Duplicated employee(s) Id passed when creating the company {e.Name}:");
            foreach(var employee in duplicatedIds)
            {
                invalidEmployeesErrorMsg.Append($"{employee} ");
            }
        }
        if (duplicatedNames.Count > 0)
        {
            invalidEmployeesErrorMsg.Append($";Duplicated employee(s) names passed when creating the company {e.Name}:");
            foreach (var employee in duplicatedNames)
            {
                invalidEmployeesErrorMsg.Append($"{employee} ");
            }
        }
        errorMessage = invalidEmployeesErrorMsg.ToString();

        return duplicatedIds.Count == 0 && duplicatedNames.Count == 0;
    }
}
