using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CompanyHandlers;

public class DeleteCompanyHandler : IRequestHandler<DeleteCommand<Company>, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteCompanyHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(DeleteCommand<Company> request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (company == null)
        {
            throw new NotFoundException(typeof(Company), request.Id);
        }

        if (company.IsDeleted)
        {
            throw new InvalidOperationException($"Company with ID {request.Id} is already deleted.");
        }

        company.IsDeleted = true;
        company.DeletedAt = DateTime.UtcNow;
        company.DeletedUserId = _currentUser.GetUserId();
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
            Message = "Successfully deleted."
        };
    }
}