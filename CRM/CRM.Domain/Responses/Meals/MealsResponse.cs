using CRM.Domain.Enums;

namespace CRM.Domain.Responses.Meals;

public class MealsResponse
{
    public Guid Id { get; set; }
    public Guid StaysId { get; set; }
    public MealsType MealsType { get; set; }
    public string Comment { get; set; }
    public bool IsDeleted { get; set; }
}