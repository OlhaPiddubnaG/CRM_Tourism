using CRM.Admin.Data.MealsDto;

namespace CRM.Admin.Data.StaysDto;

public class StaysDialogResult
{
    public StaysCreateDto[] Stays { get; set; } = Array.Empty<StaysCreateDto>();
    public MealsCreateDto[] Meals { get; set; } = Array.Empty<MealsCreateDto>();
}