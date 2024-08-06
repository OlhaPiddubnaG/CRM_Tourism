using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Meals;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.MealsHandlers;

public class GetAllMealsHandler : IRequestHandler<GetAllRequest<MealsResponse>,
    List<MealsResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllMealsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<MealsResponse>> Handle(GetAllRequest<MealsResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var meals = await _context.Meals
            .Where(m => m.Hotels.CompanyId == companyId &&
                        !m.Hotels.IsDeleted &&
                        !m.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!meals.Any())
        {
            return new List<MealsResponse>();
        }

        var mealsResponses = _mapper.Map<List<MealsResponse>>(meals);

        return mealsResponses;
    }
}