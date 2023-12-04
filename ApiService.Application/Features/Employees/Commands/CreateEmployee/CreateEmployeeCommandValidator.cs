using ApiService.Application.Contracts.Persistence;
using ApiService.Domain.Entities;
using AutoMapper;
using FluentValidation;
using System.Text;

namespace ApiService.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IMapper _mapper;

    private string employeeMail = string.Empty;
    private ICollection<string> companies = new List<string>();
    private StringBuilder invalidEmployeesErrorMsg = new();
    private string errorMessage = string.Empty;
    public CreateEmployeeCommandValidator(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
        _mapper = mapper;

        RuleFor(p => p.Email)
          .NotEmpty().WithMessage("Employee email is required.")
          .NotNull()
          .MaximumLength(50).WithMessage("Employee email must not exceed 50 characters.")
          .MinimumLength(10).WithMessage("Employee email should be at least 10 characters.");

        RuleFor(e => e)
            .MustAsync(EmailUnique)
            .WithMessage($"Employee with the same email ({employeeMail}) already exists.")
            .MustAsync(TitleUniqueInsideCompany)
            .WithMessage(x=>errorMessage);

    }
    private async Task<bool> EmailUnique(CreateEmployeeCommand e, CancellationToken token)
    {
        employeeMail = e.Email;
        return !(await _employeeRepository.EmployeeExistsAsync(e.Email));
    }

    private async Task<bool> TitleUniqueInsideCompany(CreateEmployeeCommand e, CancellationToken token)
    {
        errorMessage = string.Empty;
        await Parallel.ForEachAsync(e.Companies, async (item, cancellationToken) =>
        {
            bool exists = await _companyRepository.TitleExistsAsync(item, e.Title);
            if (exists)
            {
                Company? company = await _companyRepository.GetByIdAsync(item);
                if(company != null)
                {
                    companies.Add(company.Name);
                }
            }
        });

        if (companies.Count > 0)
        {
            invalidEmployeesErrorMsg.Append($"Invalid title {e.Title} for the employee {e.Email}. Title already exists for the companies:");
            foreach (string company in companies)
            {
                invalidEmployeesErrorMsg.Append(company);
                invalidEmployeesErrorMsg.Append(" ");
            }
        }

        errorMessage = invalidEmployeesErrorMsg.ToString();
        return companies.Count() == 0;
    }
}
