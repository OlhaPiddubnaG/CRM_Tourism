using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Company;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CompanyHandlers;

public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;

    public UpdateCompanyHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultBaseResponse> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var existingCompany = await _context.Companies
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingCompany == null)
        {
            throw new NotFoundException(typeof(Company), request.Id);
        }

        existingCompany.Name = request.Name;
        existingCompany.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Company), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}