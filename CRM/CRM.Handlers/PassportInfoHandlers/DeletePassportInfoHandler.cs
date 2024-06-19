using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class DeletePassportInfoHandler : IRequestHandler<DeleteCommand<PassportInfo>, Unit>
{
    private readonly AppDbContext _context;

    public DeletePassportInfoHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCommand<PassportInfo> request, CancellationToken cancellationToken)
    {
        var passportInfo = await _context.PassportInfo.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (passportInfo == null)
        {
            throw new NotFoundException(typeof(PassportInfo), request.Id);
        }
        if (passportInfo.IsDeleted)
        {
            throw new InvalidOperationException($"PassportInfo with ID {request.Id} is already deleted.");
        }

        passportInfo.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}