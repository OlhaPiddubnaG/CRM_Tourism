using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.NumberOfPeopleHandlers;

public class DeleteNumberOfPeopleHandler : IRequestHandler<DeleteCommand<NumberOfPeople>, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteNumberOfPeopleHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(DeleteCommand<NumberOfPeople> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var numberOfPeople = await _context.NumberOfPeople
            .Include(o => o.Order)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (numberOfPeople == null)
        {
            throw new NotFoundException(typeof(NumberOfPeople), request.Id);
        }

        if (companyId != numberOfPeople.Order.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this numberOfPeople.");
        }

        if (numberOfPeople.IsDeleted)
        {
            throw new InvalidOperationException($"NumberOfPeople with ID {request.Id} is already deleted.");
        }

        numberOfPeople.IsDeleted = true;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(NumberOfPeople), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully deleted."
        };
    }
}