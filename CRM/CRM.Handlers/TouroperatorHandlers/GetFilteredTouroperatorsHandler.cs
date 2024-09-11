using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Touroperator;
using CRM.Handlers.Services.CurrentUser;
using MediatR;

namespace CRM.Handlers.TouroperatorHandlers;

public class
    GetFilteredTouroperatorsHandler : IRequestHandler<GetFilteredAllRequest<TouroperatorResponse>,
    List<TouroperatorResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetFilteredTouroperatorsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<TouroperatorResponse>> Handle(GetFilteredAllRequest<TouroperatorResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        IQueryable<Touroperator> query = _context.Touroperators
            .Where(t => t.CompanyId == companyId &&
                        !t.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            var searchString = request.SearchString.ToLower();
            query = query.Where(t =>
                t.Name.ToLower().Contains(searchString));
        }

        var touroperatorResponses = _mapper.Map<List<TouroperatorResponse>>(query);
        return touroperatorResponses;
    }
}