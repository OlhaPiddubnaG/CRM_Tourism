using AutoMapper;
using CRM.Domain.Commands.Touroperator;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Touroperator;

namespace CRM.WebApi.Mapping;

public class TouroperatorProfile : Profile
{
    public TouroperatorProfile()
    {
        CreateMap<CreateTouroperatorCommand, Touroperator>();
        CreateMap<UpdateTouroperatorCommand, Touroperator>();
        CreateMap<Touroperator, TouroperatorResponse>();
    }
}