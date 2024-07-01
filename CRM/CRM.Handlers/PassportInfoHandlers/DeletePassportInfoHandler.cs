using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class DeletePassportInfoHandler : IRequestHandler<DeleteCommand<PassportInfo>, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeletePassportInfoHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeleteCommand<PassportInfo> request, CancellationToken cancellationToken)
    {
        var passportInfo = await _context.PassportInfo
            .Include(p => p.ClientPrivateData)
            .ThenInclude(cpd => cpd.Client)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (passportInfo == null)
        {
            throw new NotFoundException(typeof(PassportInfo), request.Id);
        }

        var currentUserCompanyId = _currentUser.GetCompanyId();

        if (passportInfo.ClientPrivateData?.Client == null)
        {
            throw new NotFoundException(typeof(ClientPrivateData), passportInfo.ClientPrivateDataId);
        }

        if (passportInfo.ClientPrivateData.Client.CompanyId != currentUserCompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete passport info for this client.");
        }

        if (passportInfo.IsDeleted)
        {
            throw new InvalidOperationException($"PassportInfo with ID {request.Id} is already deleted.");
        }

        passportInfo.IsDeleted = true;
        passportInfo.DeletedAt = DateTime.UtcNow;
        passportInfo.DeletedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(PassportInfo), ex);
        }

        return Unit.Value;
    }
}