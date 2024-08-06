using CRM.Domain.Enums;

namespace CRM.Admin.Data.NumberOfPeopleDto;

public class NumberOfPeopleUpdateDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; } 
    public int Number { get; set; }
    public ClientCategory ClientCategory { get; set; }
}