using AutoMapper;
using CRM.Domain.Commands.NumberOfPeople;
using CRM.Domain.Entities;
using CRM.Domain.Responses.NumberOfPeople;

namespace CRM.WebApi.Mapping;

public class NumberOfPeopleProfile : Profile
{
    public NumberOfPeopleProfile()
    {
        CreateMap<CreateNumberOfPeopleCommand, NumberOfPeople>();
        CreateMap<UpdateNumberOfPeopleCommand, NumberOfPeople>();
        CreateMap<NumberOfPeople, NumberOfPeopleResponse>();
    }
}