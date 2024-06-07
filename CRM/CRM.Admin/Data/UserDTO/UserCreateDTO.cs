using CRM.Domain.Enums;

namespace CRM.Admin.Data.UserDTO;

public class UserCreateDTO
{
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } 
    public string Surname { get; set; } 
    public string Email { get; set; } 
    public string Password { get; set; } 
    public ICollection<RoleType> RoleTypes { get; set; }
}