using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserHandlers;

public class DeleteUserHandler : IRequestHandler<DeleteCommand<User>, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteUserHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeleteCommand<User> request, CancellationToken cancellationToken)
    {
        var currentUserRoles = _currentUser.GetRoles();
        var currentUserCompanyId = _currentUser.GetCompanyId();

        var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.Id &&
                                                                 !c.IsDeleted, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(typeof(User), request.Id);
        }

        if (!currentUserRoles.Contains(RoleType.Admin) && currentUserCompanyId != user.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this user.");
        }

        if (user.IsDeleted)
        {
            throw new InvalidOperationException($"User with ID {request.Id} is already deleted.");
        }

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.DeletedUserId = _currentUser.GetUserId();

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