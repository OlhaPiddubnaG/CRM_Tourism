using CRM.Domain.Enums;

namespace CRM.Domain.Responses.Meals;

public class MealsResponse
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public MealsType MealsType { get; set; }
    public string Comment { get; set; }
}