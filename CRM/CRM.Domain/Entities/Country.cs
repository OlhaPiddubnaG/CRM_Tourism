using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class Country : BaseEntity
{
    public Guid CompanyId { get; set; } 
    public Company? Company { get; set; }
    public string Name { get; set; } = null!;
    public List<City> Cities { get; set; } = new();
    public Client? Client { get; set; } 
}