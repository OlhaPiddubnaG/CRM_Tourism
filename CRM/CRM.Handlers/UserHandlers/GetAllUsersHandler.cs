using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Requests;
using CRM.Domain.Responses.User;
using CRM.Handlers.Services;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserHandlers;

public class GetAllUsersHandler : IRequestHandler<GetAllRequest<UserResponse>, List<UserResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllUsersHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<UserResponse>> Handle(GetAllRequest<UserResponse> request,
        CancellationToken cancellationToken)
    {
        List<User> users;
        
        var roles = _currentUser.GetRoles();
        if (roles == RoleType.Admin)
        {
            users = await _context.Users.ToListAsync(cancellationToken);
        }
        else
        {
            var companyId = _currentUser.GetCompanyId();
            users = await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync(cancellationToken);
        }

        var userResponses = _mapper.Map<List<UserResponse>>(users);

        return userResponses;
    }
}