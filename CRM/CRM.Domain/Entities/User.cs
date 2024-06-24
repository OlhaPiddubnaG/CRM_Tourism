using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class User : Auditable
{
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiryTime { get; set; }
    public List<UserRoles> UserRoles { get; set; } = new();
    public List<UserTasks> Tasks { get; set; } = new();
    public List<Order> Orders { get; set; } = new();
    public List<Client> Clients { get; set; } = new();
}