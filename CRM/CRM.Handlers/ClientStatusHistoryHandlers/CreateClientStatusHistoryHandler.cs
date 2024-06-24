using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.ClientStatusHistory;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientStatusHistoryHandlers;

public class CreateClientStatusHistoryHandler : IRequestHandler<CreateClientStatusHistoryCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateClientStatusHistoryHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreatedResponse> Handle(CreateClientStatusHistoryCommand request,
        CancellationToken cancellationToken)
    {
        var clientStatusHistory = _mapper.Map<ClientStatusHistory>(request);
        
        clientStatusHistory.CreatedAt = DateTime.UtcNow;
        _context.ClientStatusHistory.Add(clientStatusHistory);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(ClientStatusHistory), ex);
        }

        return new CreatedResponse(clientStatusHistory.Id);
    }
}