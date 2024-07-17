using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.PassportInfo;
using CRM.Domain.Entities;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class UpdatePassportInfoHandler : IRequestHandler<UpdatePassportInfoCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdatePassportInfoHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(UpdatePassportInfoCommand request, CancellationToken cancellationToken)
    {
        var existingPassportInfo = await _context.PassportInfo
            .Include(pi => pi.ClientPrivateData)
            .ThenInclude(cpd => cpd.Client)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingPassportInfo == null)
        {
            throw new NotFoundException(typeof(PassportInfo), request.Id);
        }

        var currentUserCompanyId = _currentUser.GetCompanyId();

        if (existingPassportInfo.ClientPrivateData?.Client == null)
        {
            throw new NotFoundException(typeof(ClientPrivateData), existingPassportInfo.ClientPrivateDataId);
        }

        if (existingPassportInfo.ClientPrivateData.Client.CompanyId != currentUserCompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update passport info for this client.");
        }

        existingPassportInfo.ClientPrivateDataId = request.ClientPrivateDataId;
        existingPassportInfo.RecordNo = request.RecordNo;
        existingPassportInfo.DocumentNo = request.DocumentNo;
        existingPassportInfo.DateOfIssue = request.DateOfIssue;
        existingPassportInfo.Authority = request.Authority;
        existingPassportInfo.ExpiryDate = request.ExpiryDate;
        existingPassportInfo.Nationality = request.Nationality;
        existingPassportInfo.PlaceOfBirth = request.PlaceOfBirth;
        existingPassportInfo.TaxpayerNo = request.TaxpayerNo;
        existingPassportInfo.PassportType = request.PassportType;
        existingPassportInfo.NameLatinScript = request.NameLatinScript;
        existingPassportInfo.SurnameLatinScript = request.SurnameLatinScript;
        existingPassportInfo.UpdatedAt = DateTime.UtcNow;
        existingPassportInfo.UpdatedUserId = _currentUser.GetUserId();

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