using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.User;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserHandlers;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateUserHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUserRoles = _currentUser.GetRoles();
        var currentUserCompanyId = _currentUser.GetCompanyId();

        var existingUser = await _context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingUser == null)
        {
            throw new NotFoundException(typeof(User), request.Id);
        }

        if (!currentUserRoles.Contains(RoleType.Admin) &&
            currentUserCompanyId != existingUser.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update this user.");
        }

        existingUser.Name = request.Name;
        existingUser.Surname = request.Surname;
        existingUser.UpdatedAt = DateTime.UtcNow;
        existingUser.UpdatedUserId = _currentUser.GetUserId();
        var existingRoleTypes = existingUser.UserRoles.Select(u => u.RoleType).ToList();
        var newRoleTypes = request.RoleTypes.Except(existingRoleTypes).ToList();
        var removedRoleTypes = existingRoleTypes.Except(request.RoleTypes).ToList();

        foreach (var roleType in newRoleTypes)
        {
            existingUser.UserRoles.Add(new UserRoles { UserId = existingUser.Id, RoleType = roleType });
        }

        foreach (var roleType in removedRoleTypes)
        {
            var userRole = existingUser.UserRoles.FirstOrDefault(ur => ur.RoleType == roleType);
            if (userRole != null)
            {
                _context.UserRoles.Remove(userRole);
            }
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(User), ex);
        }

        return Unit.Value;
    }
}