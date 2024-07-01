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

public class GetPassportInfoByIdHandler : IRequestHandler<GetByIdRequest<PassportInfoResponse>, PassportInfoResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetPassportInfoByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }
        
    public async Task<PassportInfoResponse> Handle(GetByIdRequest<PassportInfoResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var passportInfo = await _context.PassportInfo
            .Include(pi => pi.ClientPrivateData)
            .ThenInclude(cpd => cpd.Client)
            .FirstOrDefaultAsync(pi => pi.Id == request.Id && pi.ClientPrivateData.Client.CompanyId == companyId && !pi.IsDeleted, cancellationToken);

        if (passportInfo == null)
        {
            throw new NotFoundException(typeof(PassportInfo), request.Id);
        }

        var passportInfoResponse = _mapper.Map<PassportInfoResponse>(passportInfo);
        return passportInfoResponse;
    }
}
