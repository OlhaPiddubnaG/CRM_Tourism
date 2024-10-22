using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Order;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace CRM.Handlers.OrderHandlers;

public class GetSortAllOrdersByClientIdHandler : IRequestHandler<GetFilteredAndSortAllWithIdRequest<OrderResponse>,
    TableData<OrderResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetSortAllOrdersByClientIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<TableData<OrderResponse>> Handle(GetFilteredAndSortAllWithIdRequest<OrderResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var clientId = request.Id;

        IQueryable<Order> query = _context.Orders
            .Include(o => o.CountryFrom)
            .Include(o => o.CountryTo)
            .Include(o => o.Stays)
            .ThenInclude(s => s.Hotel)
            .Include(o => o.Client)
            .Where(o => o.CompanyId == companyId &&
                        o.ClientId == clientId &&
                        !o.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            var searchString = request.SearchString.ToLower();
            query = query.Where(o =>
                o.Client.Name.ToLower().Contains(searchString) ||
                o.Client.Surname.ToLower().Contains(searchString) ||
                o.Comment.ToLower().Contains(searchString) ||
                o.Stays.Any(s => s.Hotel.Name.ToLower().Contains(searchString))
            );
        }

        switch (request.SortLabel)
        {
            case "Date":
                query = request.SortDirection == SortDirection.Descending
                    ? query.OrderBy(o => o.CreatedAt)
                    : query.OrderByDescending(o => o.CreatedAt);
                break;
            case "DateFrom":
                query = request.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(o => o.DateFrom).ThenBy(o => o.DateTo)
                    : query.OrderByDescending(o => o.DateFrom).ThenBy(o => o.DateTo);
                break;
            case "ClientName":
                query = request.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(o => o.Client.Name).ThenBy(o => o.Client.Surname)
                    : query.OrderByDescending(o => o.Client.Name).ThenBy(o => o.Client.Surname);
                break;
            case "CountryToName":
                query = request.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(o => o.CountryTo.Name)
                    : query.OrderByDescending(o => o.CountryTo.Name);
                break;
            case "Stays":
                query = request.SortDirection == SortDirection.Ascending
                    ? query.OrderBy(o => o.Stays.Select(s => s.Hotel.Name).FirstOrDefault())
                    : query.OrderByDescending(o => o.Stays.Select(s => s.Hotel.Name).FirstOrDefault());
                break;
            default:
                query = request.SortDirection == SortDirection.Descending
                    ? query.OrderBy(o => o.CreatedAt)
                    : query.OrderByDescending(o => o.CreatedAt);
                break;
        }

        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query.Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var orderResponses = _mapper.Map<List<OrderResponse>>(items);

        return new TableData<OrderResponse>
        {
            TotalItems = totalItems,
            Items = orderResponses
        };
    }
}