using CRM.Domain.Entities.Base;
using CRM.Domain.Enums;

namespace CRM.Domain.Entities;

public class UserRoles : BaseEntity
{
    public Guid UserId { get; set; } 
    public User? User { get; set; } 
    public RoleType RoleType { get; set; }
}