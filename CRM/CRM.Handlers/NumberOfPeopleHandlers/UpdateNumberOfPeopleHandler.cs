using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.NumberOfPeople;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.NumberOfPeopleHandlers;

public class UpdateNumberOfPeopleHandler : IRequestHandler<UpdateNumberOfPeopleCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateNumberOfPeopleHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateNumberOfPeopleCommand request,
        CancellationToken cancellationToken)
    {
        var existingNumberOfPeople = await _context.NumberOfPeople
            .Include(o => o.Order)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingNumberOfPeople == null)
        {
            throw new NotFoundException(typeof(NumberOfPeople), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (existingNumberOfPeople.Order.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update numberOfPeople.");
        }

        existingNumberOfPeople.Number = request.Number;
        existingNumberOfPeople.ClientCategory = request.ClientCategory;

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
            Message = "Successfully updated."
        };
    }
}