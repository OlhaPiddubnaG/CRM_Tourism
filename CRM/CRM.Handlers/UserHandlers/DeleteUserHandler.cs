using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Handlers.Services;
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
        var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(typeof(User), request.Id);
        }
        
        if (user.IsDeleted)
        {
            throw new InvalidOperationException($"User with ID {request.Id} is already deleted.");
        }
        
        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.DeletedUserId = _currentUser.GetUserId();
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}