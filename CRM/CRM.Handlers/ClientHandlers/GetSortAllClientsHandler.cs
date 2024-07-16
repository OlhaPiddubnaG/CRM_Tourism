using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Client;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace CRM.Handlers.ClientHandlers
{
    public class GetSortAllClientsHandler : IRequestHandler<GetFilteredAndSortAllRequest<ClientResponse>,
        TableData<ClientResponse>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;

        public GetSortAllClientsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<TableData<ClientResponse>> Handle(GetFilteredAndSortAllRequest<ClientResponse> request,
            CancellationToken cancellationToken)
        {
            var companyId = _currentUser.GetCompanyId();

            IQueryable<Client> query = _context.Clients
                .Include(u => u.Users)
                .Include(c => c.ClientStatusHistory)
                .Where(c => c.CompanyId == companyId &&
                            !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchString))
            {
                var searchString = request.SearchString.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchString) ||
                    c.Email.ToLower().Contains(searchString) ||
                    c.Surname.ToLower().Contains(searchString) ||
                    c.Phone.ToLower().Contains(searchString) ||
                    c.Users.Any(u => u.Name.ToLower().Contains(searchString))
                );
            }

            switch (request.SortLabel)
            {
                case "Name":
                    query = request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(c => c.Name)
                        : query.OrderByDescending(c => c.Name);
                    break;
                case "Surname":
                    query = request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(c => c.Surname)
                        : query.OrderByDescending(c => c.Surname);
                    break;
                case "Phone":
                    query = request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(c => c.Phone)
                        : query.OrderByDescending(c => c.Phone);
                    break;
                case "Manager":
                    query = request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(c => c.Users.FirstOrDefault().Name)
                        : query.OrderByDescending(c => c.Users.FirstOrDefault().Name);
                    break;
                case "Status":
                    query = request.SortDirection == SortDirection.Ascending
                        ? query.OrderBy(c => c.ClientStatusHistory.OrderByDescending(h => h.ClientStatus).FirstOrDefault().ClientStatus)
                        : query.OrderByDescending(c => c.ClientStatusHistory.OrderByDescending(h => h.ClientStatus).FirstOrDefault().ClientStatus);
                    break;
                default:
                    query = query.OrderBy(c => c.Name);
                    break;
            }

            var totalItems = await query.CountAsync(cancellationToken);
            var items = await query.Skip(request.Page * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var clientResponses = _mapper.Map<List<ClientResponse>>(items);

            return new TableData<ClientResponse>
            {
                TotalItems = totalItems,
                Items = clientResponses
            };
        }
    }
}