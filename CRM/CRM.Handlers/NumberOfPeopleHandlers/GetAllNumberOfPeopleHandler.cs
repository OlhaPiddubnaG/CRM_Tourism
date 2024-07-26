using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.NumberOfPeople;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.NumberOfPeopleHandlers;

public class GetAllNumberOfPeopleHandler : IRequestHandler<GetAllRequest<NumberOfPeopleResponse>,
    List<NumberOfPeopleResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllNumberOfPeopleHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<NumberOfPeopleResponse>> Handle(GetAllRequest<NumberOfPeopleResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var numberOfPeople = await _context.NumberOfPeople
            .Where(o => o.Order.CompanyId == companyId &&
                        !o.Order.IsDeleted  &&
                        !o.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!numberOfPeople.Any())
        {
            return new List<NumberOfPeopleResponse>();
        }

        var numberOfPeopleResponses = _mapper.Map<List<NumberOfPeopleResponse>>(numberOfPeople);

        return numberOfPeopleResponses;
    }
}