using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Stays;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.StaysHandlers;

public class GetAllStaysHandler : IRequestHandler<GetAllRequest<StaysResponse>,
    List<StaysResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllStaysHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<StaysResponse>> Handle(GetAllRequest<StaysResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var stays = await _context.Stays
            .Include(s => s.Order)
            .Where(s => s.Order.CompanyId == companyId &&
                        !s.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!stays.Any())
        {
            return new List<StaysResponse>();
        }

        var staysResponses = _mapper.Map<List<StaysResponse>>(stays);

        return staysResponses;
    }
}