using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Meals;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.MealsHandlers;

public class CreateMealsHandler : IRequestHandler<CreateMealsCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateMealsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateMealsCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var stays = await _context.Stays
            .FirstOrDefaultAsync(s => s.Id == request.StaysId &&
                                      s.Order.CompanyId == companyId &&
                                      !s.IsDeleted, cancellationToken);

        if (stays == null)
        {
            throw new InvalidOperationException(
                "Stays not found or user is not authorized to create meals for stays from a different company.");
        }

        var meals = _mapper.Map<Meals>(request);

        _context.Meals.Add(meals);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Meals), ex);
        }

        return new CreatedResponse(meals.Id);
    }
}