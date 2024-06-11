using CRM.Domain.Enums;

namespace CRM.Admin.Data.UserDTO;

public class UserUpdateDTO : IUserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string Surname { get; set; }
    public IEnumerable<RoleType> RoleTypes { get; set; } = new List<RoleType>();
}