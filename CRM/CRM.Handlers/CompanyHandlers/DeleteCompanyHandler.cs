using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Handlers.Services;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CompanyHandlers;

public class DeleteCompanyHandler : IRequestHandler<DeleteCommand<Company>, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteCompanyHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeleteCommand<Company> request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (company == null)
        {
            throw new NotFoundException(typeof(Company), request.Id);
        }

        company.IsDeleted = true;
        company.DeletedAt = DateTime.UtcNow;
        company.DeletedUserId = _currentUser.GetUserId();
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}