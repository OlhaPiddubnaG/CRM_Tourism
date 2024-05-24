using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class City : BaseEntity
{
    public Guid CompanyId { get; set; } 
    public Company? Company { get; set; }
    public Guid CountryId { get; set; } 
    public Country? Country { get; set; }
    public string Name { get; set; } 
}