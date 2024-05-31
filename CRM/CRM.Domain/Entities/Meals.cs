using CRM.Domain.Entities.Base;
using CRM.Domain.Enums;

namespace CRM.Domain.Entities;

public class Meals : BaseEntity
{
    public Guid StaysId { get; set; } 
    public Stays Stays { get; set; } 
    public MealsType MealsType { get; set; }
    public string Comment { get; set; }
}