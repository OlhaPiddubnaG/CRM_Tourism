using AutoMapper;
using CRM.Domain.Commands.Meals;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Meals;

namespace CRM.WebApi.Mapping;

public class MealsProfile : Profile
{
    public MealsProfile()
    {
        CreateMap<CreateMealsCommand, Meals>();
        CreateMap<UpdateMealsCommand, Meals>();
        CreateMap<Meals, MealsResponse>();
    }
}