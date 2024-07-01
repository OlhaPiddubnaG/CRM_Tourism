using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.ClientPrivateData;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientPrivateDataHandlers;

public class GetAllClientPrivateDatasHandler : IRequestHandler<GetAllRequest<ClientPrivateDataResponse>, List<ClientPrivateDataResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllClientPrivateDatasHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<ClientPrivateDataResponse>> Handle(GetAllRequest<ClientPrivateDataResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var clientPrivateDatas = await _context.ClientPrivateDatas
            .Include(cpd => cpd.Client)
            .Where(cpd => !cpd.IsDeleted && cpd.Client.CompanyId == companyId)
            .ToListAsync(cancellationToken);

        if (clientPrivateDatas == null)
        {
            throw new UnauthorizedAccessException("User is not authorized to access client private data.");
        }

        var clientPrivateDataResponses = _mapper.Map<List<ClientPrivateDataResponse>>(clientPrivateDatas);

        return clientPrivateDataResponses;
    }
}