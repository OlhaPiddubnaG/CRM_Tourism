using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.PassportInfo;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class UpdatePassportInfoHandler : IRequestHandler<UpdatePassportInfoCommand, Unit>
{
    private readonly AppDbContext _context;

    public UpdatePassportInfoHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePassportInfoCommand request, CancellationToken cancellationToken)
    {
        var existingPassportInfo = await _context.PassportInfo
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingPassportInfo == null)
        {
            throw new NotFoundException(typeof(PassportInfo), request.Id);
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