using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.PassportInfo;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class
    GetAllPassportsInfoHandler : IRequestHandler<GetAllRequest<PassportInfoResponse>, List<PassportInfoResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllPassportsInfoHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<PassportInfoResponse>> Handle(GetAllRequest<PassportInfoResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var passportInfos = await _context.PassportInfo
            .Include(pi => pi.ClientPrivateData)
            .ThenInclude(cpd => cpd.Client)
            .Where(pi => pi.ClientPrivateData.Client.CompanyId == companyId &&
                         !pi.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!passportInfos.Any())
        {
            return new List<PassportInfoResponse>();
        }

        var passportInfoResponses = _mapper.Map<List<PassportInfoResponse>>(passportInfos);

        return passportInfoResponses;
    }
}