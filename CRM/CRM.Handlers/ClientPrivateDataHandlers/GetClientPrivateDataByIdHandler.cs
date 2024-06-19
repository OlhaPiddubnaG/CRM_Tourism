using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.ClientPrivateData;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientPrivateDataHandlers;

public class GetClientPrivateDataByIdHandler : IRequestHandler<GetByIdRequest<ClientPrivateDataResponse>, ClientPrivateDataResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetClientPrivateDataByIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ClientPrivateDataResponse> Handle(GetByIdRequest<ClientPrivateDataResponse> request,
        CancellationToken cancellationToken)
    {
        var clientPrivateData =  await _context.ClientPrivateDatas.FirstAsync(c => c.Id == request.Id , cancellationToken);

        if (clientPrivateData == null)
        {
            throw new NotFoundException(typeof(ClientPrivateData), request.Id);
        }

        var clientPrivateDataResponse = _mapper.Map<ClientPrivateDataResponse>(clientPrivateData);
        return clientPrivateDataResponse;
    }
}