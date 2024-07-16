using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Client;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientHandlers;

public class GetClientByIdHandler : IRequestHandler<GetByIdRequest<ClientResponse>, ClientResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetClientByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<ClientResponse> Handle(GetByIdRequest<ClientResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var client = await _context.Clients
            .Include(u => u.Users)
            .Include(c => c.ClientStatusHistory)
            .FirstOrDefaultAsync(c => c.CompanyId == companyId &&
                                      c.Id == request.Id &&
                                      !c.IsDeleted, cancellationToken);

        if (client == null)
        {
            throw new NotFoundException(typeof(Client), request.Id);
        }

        var clientResponse = _mapper.Map<ClientResponse>(client);
        return clientResponse;
    }
}