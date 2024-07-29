using CRM.Domain.Enums;

namespace CRM.Admin.Data.MealsDto;

public class MealsCreateDto
{
    public Guid StaysId { get; set; } 
    public MealsType MealsType { get; set; }
    public string Comment { get; set; }
}