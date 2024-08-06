using CRM.Domain.Enums;

namespace CRM.Admin.Data.MealsDto;

public class MealsUpdateDto
{
    public Guid Id { get; set; }
    public Guid HotelId { get; set; }
    public MealsType MealsType { get; set; }
    public string Comment { get; set; }
}