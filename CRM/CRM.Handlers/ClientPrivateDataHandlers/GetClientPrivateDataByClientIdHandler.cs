using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.ClientPrivateData;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientPrivateDataHandlers;

public class GetClientPrivateDataByClientIdHandler : IRequestHandler<GetByIdRequest<ClientPrivateDataResponse>,
    ClientPrivateDataResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetClientPrivateDataByClientIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ClientPrivateDataResponse> Handle(GetByIdRequest<ClientPrivateDataResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var clientPrivateData = await _context.ClientPrivateDatas
            .Include(cpd => cpd.Client)
            .FirstOrDefaultAsync(cpd => cpd.ClientId == request.Id &&
                                        !cpd.IsDeleted &&
                                        cpd.Client.CompanyId == companyId, cancellationToken);

        if (clientPrivateData == null)
        {
            throw new NotFoundException(typeof(ClientPrivateData), request.Id);
        }

        var clientPrivateDataResponse = _mapper.Map<ClientPrivateDataResponse>(clientPrivateData);
        return clientPrivateDataResponse;
    }
}