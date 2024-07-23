using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class Company : Auditable
{
    public string Name { get; set; } = null!;
    public List<User> Users { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
    public List<Client> Clients { get; set; } = new();
    public List<Country> Countries { get; set; } = new();
    public List<City> Cities { get; set; } = new();
    public List<Touroperator> Touroperators { get; set; } = new();
}