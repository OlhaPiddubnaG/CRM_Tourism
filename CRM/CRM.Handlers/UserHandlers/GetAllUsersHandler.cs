using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Constants;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Requests;
using CRM.Domain.Responses.User;
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
        var users = await GetUsersByRoleAsync(cancellationToken);
        if (!users.Any())
        {
            return new List<UserResponse>();
        }
        
        var userResponses = _mapper.Map<List<UserResponse>>(users);
        return userResponses;
    }

    private async Task<List<User>> GetUsersByRoleAsync(CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        
        var roles = _currentUser.GetRoles();
        if (roles.Contains(RoleType.Admin))
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .Where(u => !u.IsDeleted &&
                            u.Name != Constants.DefaultAdminUserName)
                .ToListAsync(cancellationToken);
        }  
        if (roles.Contains(RoleType.CompanyAdmin))
        {
            return  await _context.Users
                .Include(u => u.UserRoles)
                .Where(u => u.CompanyId == companyId &&
                            !u.IsDeleted)
                .ToListAsync(cancellationToken);
        }
        
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .Where(u => u.CompanyId == companyId &&
                        !u.IsDeleted &&
                        u.Name != Constants.DefaultAdminUserName)
            .ToListAsync(cancellationToken);

        if (users == null)
        {
            throw new UnauthorizedAccessException("User is not authorized to access user data.");
        }

        return users;
    }
}