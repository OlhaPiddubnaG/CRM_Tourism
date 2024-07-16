using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Client;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientHandlers;

public class GetAllClientsHandler : IRequestHandler<GetAllRequest<ClientResponse>, List<ClientResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllClientsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<ClientResponse>> Handle(GetAllRequest<ClientResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var clients = await _context.Clients
            .Include(u => u.Users)
            .Include(c => c.ClientStatusHistory)
            .Where(c => c.CompanyId == companyId &&
                        !c.IsDeleted)
            .ToListAsync(cancellationToken);

        if (clients == null)
        {
            throw new UnauthorizedAccessException("User is not authorized to access client data.");
        }

        var clientResponses = _mapper.Map<List<ClientResponse>>(clients);

        foreach (var clientResponse in clientResponses)
        {
            var client = clients.FirstOrDefault(c => c.Id == clientResponse.Id);
            if (client != null)
            {
                clientResponse.ManagerNames = client.Users
                    .Select(u => $"{u.Name} {u.Surname}")
                    .ToList();

                clientResponse.LatestStatus = client.CurrentStatus;
            }
        }

        return clientResponses;
    }
}