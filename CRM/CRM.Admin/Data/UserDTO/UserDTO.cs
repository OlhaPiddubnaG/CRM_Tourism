using CRM.Domain.Enums;

namespace CRM.Admin.Data.UserDTO;

public class UserDTO
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } 
    public string Surname { get; set; } 
    public string Email { get; set; } 
    public string Password { get; set; } 
    public bool IsDeleted { get; set; } 
    public IEnumerable<RoleType> RoleTypes { get; set; }
}