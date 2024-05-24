using AutoMapper;
using CRM.Domain.Commands.Company;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Company;

namespace CRM.WebApi.Mapping;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CreateCompanyCommand, Company>();
        CreateMap<UpdateCompanyCommand, Company>();
        CreateMap<Company, CompanyResponse>();
    }
}