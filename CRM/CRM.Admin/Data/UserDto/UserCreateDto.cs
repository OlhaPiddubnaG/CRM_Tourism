using CRM.Domain.Enums;

namespace CRM.Admin.Data.UserDto;

public class UserCreateDto
{
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } 
    public string Surname { get; set; } 
    public string Email { get; set; } 
    public string Password { get; set; } 
    public IEnumerable<RoleType> RoleTypes { get; set; }
}