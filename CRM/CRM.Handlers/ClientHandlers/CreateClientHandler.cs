using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Client;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientHandlers;

public class CreateClientHandler : IRequestHandler<CreateClientCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateClientHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreatedResponse> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        request.Name = request.Name.ToUpper();
        var client = _mapper.Map<Client>(request);
        
        client.CreatedAt = DateTime.UtcNow;
        _context.Clients.Add(client);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Client), ex);
        }

        return new CreatedResponse(client.Id);
    }
}