using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.ClientPrivateData;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientPrivateDataHandlers;

public class CreateClientPrivateDataHandler : IRequestHandler<CreateClientPrivateDataCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateClientPrivateDataHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreatedResponse> Handle(CreateClientPrivateDataCommand request, CancellationToken cancellationToken)
    {
        var clientPrivateData = _mapper.Map<ClientPrivateData>(request);
        
        clientPrivateData.CreatedAt = DateTime.UtcNow;
        _context.ClientPrivateDatas.Add(clientPrivateData);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(ClientPrivateData), ex);
        }

        return new CreatedResponse(clientPrivateData.Id);
    }

}