using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Client;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientHandlers;

public class CreateClientHandler : IRequestHandler<CreateClientCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateClientHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        request.Name = request.Name.ToUpper();
        request.Surname = request.Surname.ToUpper();
        request.Patronymic = request.Patronymic.ToUpper();

        var client = _mapper.Map<Client>(request);

        if (companyId != request.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to create this client.");
        }

        if (request.ManagerIds.Any())
        {
            var users = await _context.Users
                .Where(u => request.ManagerIds.Contains(u.Id) &&
                            u.CompanyId == companyId)
                .ToListAsync(cancellationToken);

            if (users.Count != request.ManagerIds.Count)
            {
                throw new KeyNotFoundException("One or more user IDs are invalid.");
            }

            client.Users.AddRange(users);
        }

        client.CreatedAt = DateTime.UtcNow;
        client.CreatedUserId = _currentUser.GetUserId();
        client.CurrentStatus = request.LatestStatus;

        var clientStatusHistory = new ClientStatusHistory
        {
            ClientId = client.Id,
            DateTime = DateTime.UtcNow,
            ClientStatus = request.LatestStatus,
            CreatedAt = DateTime.UtcNow,
            CreatedUserId = _currentUser.GetUserId()
        };

        client.ClientStatusHistory.Add(clientStatusHistory);

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