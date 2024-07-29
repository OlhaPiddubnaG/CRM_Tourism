using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Meals;

public class UpdateMealsCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid StaysId { get; set; }
    public MealsType MealsType { get; set; }
    public string Comment { get; set; }
}