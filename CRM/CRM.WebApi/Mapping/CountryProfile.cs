using AutoMapper;
using CRM.Domain.Commands.Country;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Сountry;

namespace CRM.WebApi.Mapping;

public class CountryProfile: Profile
{
    public CountryProfile()
    {
        CreateMap<CreateCountryCommand, Country>();
        CreateMap<UpdateCountryCommand, Country>();
        CreateMap<Country, CountryResponse>();
    }
}