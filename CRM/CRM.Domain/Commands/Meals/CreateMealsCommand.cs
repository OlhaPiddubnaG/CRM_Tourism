using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Meals;

public class CreateMealsCommand : IRequest<CreatedResponse>
{
    public Guid HotelId { get; set; }
    public MealsType MealsType { get; set; }
    public string Comment { get; set; } = "";
}