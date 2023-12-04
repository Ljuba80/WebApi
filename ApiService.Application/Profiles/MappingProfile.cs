using ApiService.Application.Features.Companies.Commands.CreateCompany;
using ApiService.Application.Features.Employees.Commands.CreateEmployee;
using ApiService.Domain.Entities;
using AutoMapper;

namespace ApiService.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile() {
        CreateMap<CreateCompanyCommand, Company>();
        CreateMap<CreateEmployeeCommand, Employee>()
            .ForMember(dest => dest.Companies, act => act.Ignore());
        CreateMap<Company, CompanyDto>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest=>dest.Companies, act=>act.Ignore())
            .ForMember(dest=>dest.Id, act=>act.Ignore())
            .ForMember(dest=>dest.CreatedAt, act=>act.Ignore());
    }
}
