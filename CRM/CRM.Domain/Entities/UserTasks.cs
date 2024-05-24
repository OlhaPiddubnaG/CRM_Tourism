using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class UserTasks : Auditable
{
    public Guid UserId { get; set; } 
    public User? User { get; set; } 
    public DateTime DateTime { get; set; } 
    public string Description { get; set; } 
    public TaskStatus TaskStatus { get; set; } 
}