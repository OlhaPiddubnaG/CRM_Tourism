using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Touroperator;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.TouroperatorHandlers;

public class
    GetAllTouroperatorsHandler : IRequestHandler<GetAllRequest<TouroperatorResponse>, List<TouroperatorResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllTouroperatorsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<TouroperatorResponse>> Handle(GetAllRequest<TouroperatorResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var touroperators = await _context.Touroperators
            .Where(t => t.CompanyId == companyId &&
                        !t.IsDeleted)
            .ToListAsync(cancellationToken);
        
        if (!touroperators.Any())
        {
            return new List<TouroperatorResponse>();
        }

        var touroperatorResponses = _mapper.Map<List<TouroperatorResponse>>(touroperators);

        return touroperatorResponses;
    }
}