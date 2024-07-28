using CRM.Domain.Enums;

namespace CRM.Admin.Data.UserDto;

public class UserUpdateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string Surname { get; set; }
    public IEnumerable<RoleType> RoleTypes { get; set; } = new List<RoleType>();
}