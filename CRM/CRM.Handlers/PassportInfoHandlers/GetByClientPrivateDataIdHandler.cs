using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.PassportInfo;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PassportInfoHandlers;

public class GetByClientPrivateDataIdHandler : IRequestHandler<GetByIdReturnListRequest<PassportInfoResponse>, List<PassportInfoResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetByClientPrivateDataIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }
        
    public async Task<List<PassportInfoResponse>> Handle(GetByIdReturnListRequest<PassportInfoResponse> request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var passportInfoList = await _context.PassportInfo
            .Include(pi => pi.ClientPrivateData)
            .ThenInclude(cpd => cpd.Client)
            .Where(pi => pi.ClientPrivateDataId == request.Id && pi.ClientPrivateData.Client.CompanyId == companyId && !pi.IsDeleted)
            .ToListAsync(cancellationToken);

        if (passportInfoList == null || !passportInfoList.Any())
        {
            throw new NotFoundException(typeof(PassportInfo), request.Id);
        }

        var passportInfoResponseList = _mapper.Map<List<PassportInfoResponse>>(passportInfoList);
        return passportInfoResponseList;
    }
}
