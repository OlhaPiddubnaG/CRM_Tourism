using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Touroperator;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.TouroperatorHandlers;

public class GetTouroperatorByIdHandler : IRequestHandler<GetByIdRequest<TouroperatorResponse>, TouroperatorResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetTouroperatorByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<TouroperatorResponse> Handle(GetByIdRequest<TouroperatorResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var touroperator = await _context.Touroperators
            .FirstOrDefaultAsync(c => c.CompanyId == companyId &&
                                      c.Id == request.Id &&
                                      !c.IsDeleted, cancellationToken);

        if (touroperator == null)
        {
            throw new UnauthorizedAccessException("User is not authorized to access touroperator.");
        }

        var touroperatorResponse = _mapper.Map<TouroperatorResponse>(touroperator);

        return touroperatorResponse;
    }
}