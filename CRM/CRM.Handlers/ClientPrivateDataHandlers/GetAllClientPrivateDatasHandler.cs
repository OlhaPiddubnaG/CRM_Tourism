using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.ClientPrivateData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientPrivateDataHandlers;

public class GetAllClientPrivateDatasHandler : IRequestHandler<GetAllRequest<ClientPrivateDataResponse>, List<ClientPrivateDataResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllClientPrivateDatasHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ClientPrivateDataResponse>> Handle(GetAllRequest<ClientPrivateDataResponse> request,
        CancellationToken cancellationToken)
    {
        var clientsPrivateData = await _context.ClientPrivateDatas.ToListAsync(cancellationToken);

        var clientPrivateDataResponses = _mapper.Map<List<ClientPrivateDataResponse>>(clientsPrivateData);
        return clientPrivateDataResponses;
    }
}