using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Meals;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.MealsHandlers;

public class GetMealsByIdHandler : IRequestHandler<GetByIdRequest<MealsResponse>,
    MealsResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetMealsByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<MealsResponse> Handle(GetByIdRequest<MealsResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var meals = await _context.Meals
            .FirstOrDefaultAsync(m => m.Id == request.Id &&
                                      m.Hotels.CompanyId == companyId &&
                                      !m.Hotels.IsDeleted &&
                                      !m.IsDeleted);

        if (meals == null)
        {
            throw new NotFoundException(typeof(Meals), request.Id);
        }

        var mealsResponse = _mapper.Map<MealsResponse>(meals);

        return mealsResponse;
    }
}