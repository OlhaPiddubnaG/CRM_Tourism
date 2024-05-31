using AutoMapper;
using CRM.Domain.Commands.User;
using CRM.Domain.Entities;
using CRM.Domain.Responses.User;

namespace CRM.WebApi.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, User>();
        CreateMap<UpdateUserCommand, User>();
        CreateMap<User, UserResponse>();
    } 
}