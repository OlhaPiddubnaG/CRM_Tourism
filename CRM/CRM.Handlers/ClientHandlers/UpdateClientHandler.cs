using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Client;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientHandlers;

public class UpdateClientHandler : IRequestHandler<UpdateClientCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateClientHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var existingClient = await _context.Clients
            .Include(c => c.Users)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingClient == null)
        {
            throw new NotFoundException(typeof(Client), request.Id);
        }

        if (companyId != existingClient.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update this client.");
        }

        existingClient.Name = request.Name;
        existingClient.Surname = request.Surname;
        existingClient.Patronymic = request.Patronymic;
        existingClient.DateOfBirth = request.DateOfBirth;
        existingClient.Address = request.Address;
        existingClient.Email = request.Email;
        existingClient.Phone = request.Phone;
        existingClient.Comment = request.Comment;
        existingClient.UpdatedAt = DateTime.UtcNow;
        existingClient.UpdatedUserId = _currentUser.GetUserId();

        if (request.ManagerIds != null && request.ManagerIds.Any())
        {
            var newManagers = await _context.Users
                .Where(u => request.ManagerIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            if (newManagers.Count != request.ManagerIds.Count)
            {
                throw new KeyNotFoundException("One or more user IDs are invalid.");
            }

            existingClient.Users.Clear();
            existingClient.Users.AddRange(newManagers);
        }
        
        if (existingClient.CurrentStatus != request.LatestStatus)
        {
            existingClient.CurrentStatus = request.LatestStatus;
            var clientStatusHistory = new ClientStatusHistory
            {
                ClientId = existingClient.Id,
                DateTime = DateTime.UtcNow,
                ClientStatus = request.LatestStatus,
                CreatedAt = DateTime.UtcNow,
                CreatedUserId = _currentUser.GetUserId()
            };
            existingClient.ClientStatusHistory.Add(clientStatusHistory);
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Client), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}