using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using MediatR;

namespace CRM.Handlers.CompanyHandlers;

public class DeleteCompanyHandler : IRequestHandler<DeleteCommand<Company>, Unit>
{
    private readonly AppDbContext _context;

    public DeleteCompanyHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCommand<Company> request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FindAsync(new object[] { request.Id }, cancellationToken);
        if (company == null)
        {
            throw new NotFoundException(typeof(Company), request.Id);
        }

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}