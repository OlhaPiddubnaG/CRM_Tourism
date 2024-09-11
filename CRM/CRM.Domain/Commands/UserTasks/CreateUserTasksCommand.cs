using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.UserTasks;

public class CreateUserTasksCommand : IRequest<CreatedResponse>
{
    public Guid UserId { get; set; } 
    public DateTime DateTime { get; set; } 
    public string Description { get; set; } 
    public UserTaskStatus TaskStatus { get; set; } 
}