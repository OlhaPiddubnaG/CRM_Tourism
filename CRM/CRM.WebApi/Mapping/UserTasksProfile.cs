using AutoMapper;
using CRM.Domain.Commands.UserTasks;
using CRM.Domain.Entities;
using CRM.Domain.Responses.UserTasks;

namespace CRM.WebApi.Mapping;

public class UserTasksProfile : Profile
{
    public UserTasksProfile()
    {
        CreateMap<CreateUserTasksCommand, UserTasks>();
        CreateMap<UpdateUserTasksCommand, UserTasks>();
        CreateMap<UserTasks, UserTasksResponse>();
    } 
}