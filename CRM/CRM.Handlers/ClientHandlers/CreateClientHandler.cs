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
        request.Surname = request.Surname.ToUpper();

        var client = _mapper.Map<Client>(request);

        if (request.ManagerIds.Any())
        {
            var users = await _context.Users
                .Where(u => request.ManagerIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            if (users.Count != request.ManagerIds.Count)
            {
                throw new KeyNotFoundException("One or more user IDs are invalid.");
            }

            client.Users.AddRange(users);
        }

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