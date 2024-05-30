using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserHandlers;

public class DeleteUserHandler : IRequestHandler<DeleteCommand<User>, Unit>
{
    private readonly AppDbContext _context;

    public DeleteUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCommand<User> request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (user == null)
        {
            throw new NotFoundException(typeof(User), request.Id);
        }
        
        user.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}