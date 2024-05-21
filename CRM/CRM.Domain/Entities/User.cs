using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class User : Auditable
{
    public Guid CompanyId { get; set; }
    public Company? Company  {get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } 
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public List<Role> Roles { get; set; } = new();
    public List<Task> Tasks { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
}