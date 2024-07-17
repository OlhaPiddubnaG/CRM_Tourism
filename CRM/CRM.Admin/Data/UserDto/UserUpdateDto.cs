using CRM.Domain.Enums;

namespace CRM.Admin.Data.UserDto;

public class UserUpdateDto : IUserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string Surname { get; set; }
    public IEnumerable<RoleType> RoleTypes { get; set; } = new List<RoleType>();
}