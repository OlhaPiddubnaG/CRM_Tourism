using CRM.Domain.Enums;

namespace CRM.Domain.Responses.NumberOfPeople;

public class NumberOfPeopleResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public int Number { get; set; }
    public ClientCategory ClientCategory { get; set; }
    public bool IsDeleted { get; set; }
}