using AutoMapper;
using CRM.Core.Exceptions;
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

public class GetUserByIdHandler : IRequestHandler<GetByIdRequest<UserResponse>, UserResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetUserByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<UserResponse> Handle(GetByIdRequest<UserResponse> request,
        CancellationToken cancellationToken)
    {
        User user;
        var roles = _currentUser.GetRoles();
        if (roles == RoleType.Admin)
        {
            user = await _context.Users
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        }
        else
        {
            var companyId = _currentUser.GetCompanyId();
            user = await _context.Users
                .FirstOrDefaultAsync(c => c.CompanyId == companyId && c.Id == request.Id, cancellationToken);
        }

        if (user == null)
        {
            throw new NotFoundException(typeof(User), request.Id);
        }

        var userResponse = _mapper.Map<UserResponse>(user);
        
        return userResponse;
    }
}