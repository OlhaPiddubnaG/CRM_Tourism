using CRM.Domain.Enums;

namespace CRM.Admin.Data.UserTasksDto;

public class UserTasksCreateDto
{
    public Guid UserId { get; set; } 
    public DateTime DateTime { get; set; }
    public string Description { get; set; } 
    public UserTaskStatus TaskStatus { get; set; } 
}