using CRM.Domain.Enums;

namespace CRM.Admin.Data.UserTasksDto;

public class UserTasksUpdateDto
{
    public Guid Id { get; set; } 
    public Guid UserId { get; set; } 
    public DateTime DateTime { get; set; } 
    public string Description { get; set; } 
    public UserTaskStatus TaskStatus { get; set; } 
}