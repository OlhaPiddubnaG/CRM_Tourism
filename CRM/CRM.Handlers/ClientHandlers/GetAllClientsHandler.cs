using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
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
        var clients = await GetClientsByRoleAsync(cancellationToken);

        var clientResponses = _mapper.Map<List<ClientResponse>>(clients);
        return clientResponses;
    }

    private async Task<List<Client>> GetClientsByRoleAsync(CancellationToken cancellationToken)
    {
        var roles = _currentUser.GetRoles();
        if (roles.Contains(RoleType.Admin))
        {
            return await _context.Clients.ToListAsync(cancellationToken);
        }

        var companyId = _currentUser.GetCompanyId();
        return await _context.Clients.Where(c => c.CompanyId == companyId).ToListAsync(cancellationToken);
    }
}