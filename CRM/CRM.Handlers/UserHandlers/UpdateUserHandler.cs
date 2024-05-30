using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.User;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserHandlers;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly AppDbContext _context;

    public UpdateUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingUser == null)
        {
            throw new NotFoundException(typeof(User), request.Id);
        }
        existingUser.Name = request.Name;
        existingUser.Surname = request.Surname;
        
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