using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Meals;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.MealsHandlers;

public class UpdateMealsHandler : IRequestHandler<UpdateMealsCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateMealsHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateMealsCommand request,
        CancellationToken cancellationToken)
    {
        var existingMeals = await _context.Meals
            .Include(m => m.Hotels)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (existingMeals == null)
        {
            throw new NotFoundException(typeof(Meals), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (existingMeals.Hotels.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update meals.");
        }

        existingMeals.HotelId = request.HotelId;
        existingMeals.MealsType = request.MealsType;
        existingMeals.Comment = request.Comment;

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
            Message = "Successfully updated."
        };
    }
}