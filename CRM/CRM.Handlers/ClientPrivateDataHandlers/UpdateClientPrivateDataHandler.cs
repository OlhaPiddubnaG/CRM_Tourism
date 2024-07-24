using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.ClientPrivateData;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientPrivateDataHandlers;

public class UpdateClientPrivateDataHandler : IRequestHandler<UpdateClientPrivateDataCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateClientPrivateDataHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateClientPrivateDataCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var existingClientPrivateData = await _context.ClientPrivateDatas
            .Include(cpd => cpd.Client)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingClientPrivateData == null)
        {
            throw new NotFoundException(typeof(ClientPrivateData), request.Id);
        }

        if (companyId != existingClientPrivateData.Client?.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update this client private data.");
        }

        existingClientPrivateData.UpdatedAt = DateTime.UtcNow;
        existingClientPrivateData.UpdatedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(ClientPrivateData), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}