using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.UserTasks;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace CRM.Handlers.UserTasksHandlers;

public class GetSortAllUserTasksHandler : IRequestHandler<GetFilteredAndSortAllWithIdRequest<UserTasksResponse>, TableData<UserTasksResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetSortAllUserTasksHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<TableData<UserTasksResponse>> Handle(GetFilteredAndSortAllWithIdRequest<UserTasksResponse> request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var userId = request.Id;
        var searchString = request.SearchString?.ToLower();

        IQueryable<UserTasks> query = _context.UserTasks
            .Include(u => u.User)
            .Where(u => u.User.CompanyId == companyId &&
                        u.UserId == userId &&
                        !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            query = query.Where(u =>
                u.Description.ToLower().Contains(searchString));
        }
        
        query = request.SortLabel switch
        {
            "Date" => request.SortDirection == SortDirection.Descending ? query.OrderBy(u => u.DateTime) : query.OrderByDescending(u => u.DateTime),
            "Description" => request.SortDirection == SortDirection.Ascending ? query.OrderBy(u => u.Description) : query.OrderByDescending(u => u.Description),
            "TaskStatus" => request.SortDirection == SortDirection.Ascending ? query.OrderBy(u => u.TaskStatus) : query.OrderByDescending(u => u.TaskStatus),
            _ => request.SortDirection == SortDirection.Descending ? query.OrderBy(u => u.DateTime) : query.OrderByDescending(u => u.DateTime)
        };

        var totalItems = await query.CountAsync(cancellationToken);
        var items = await query.Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var tasksResponses = _mapper.Map<List<UserTasksResponse>>(items);

        return new TableData<UserTasksResponse>
        {
            TotalItems = totalItems,
            Items = tasksResponses
        };
    }
}