using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.MealsHandlers;

public class DeleteMealsHandler : IRequestHandler<DeleteCommand<Meals>, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteMealsHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(DeleteCommand<Meals> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var meals = await _context.Meals
            .Include(m => m.Stays)
            .ThenInclude(s => s.Order)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (meals == null)
        {
            throw new NotFoundException(typeof(Meals), request.Id);
        }

        if (companyId != meals.Stays.Order.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this meals.");
        }

        if (meals.IsDeleted)
        {
            throw new InvalidOperationException($"Meals with ID {request.Id} is already deleted.");
        }

        meals.IsDeleted = true;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Meals), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully deleted."
        };
    }
}