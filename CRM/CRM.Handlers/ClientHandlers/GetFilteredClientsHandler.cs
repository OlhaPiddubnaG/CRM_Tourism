using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Client;
using CRM.Handlers.Services.CurrentUser;
using MediatR;

namespace CRM.Handlers.ClientHandlers;

public class GetFilteredClientsHandler : IRequestHandler<GetFilteredAllRequest<ClientResponse>, List<ClientResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetFilteredClientsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<ClientResponse>> Handle(GetFilteredAllRequest<ClientResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        IQueryable<Client> query = _context.Clients
            .Where(c => c.CompanyId == companyId &&
                        !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            var searchString = request.SearchString.ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(searchString) ||
                c.Surname.ToLower().Contains(searchString));
        }

        var clientResponses = _mapper.Map<List<ClientResponse>>(query);
        return clientResponses;
    }
}