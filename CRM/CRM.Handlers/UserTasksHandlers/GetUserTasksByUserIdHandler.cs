using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.UserTasks;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserTasksHandlers;

public class GetUserTasksByUserIdHandler : IRequestHandler<GetByIdReturnListRequest<UserTasksResponse>,
    List<UserTasksResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetUserTasksByUserIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<UserTasksResponse>> Handle(GetByIdReturnListRequest<UserTasksResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var userId = request.Id;
        var userTasks = await _context.UserTasks
            .Include(u => u.User )
            .Where(u => u.User.CompanyId == companyId &&
                        u.UserId == userId && 
                        !u.User.IsDeleted  &&
                        !u.IsDeleted)
            .OrderByDescending(u => u.DateTime)
            .ToListAsync(cancellationToken);

        if (!userTasks.Any())
        {
            return new List<UserTasksResponse>();
        }

        var userTasksResponses = _mapper.Map<List<UserTasksResponse>>(userTasks);

        return userTasksResponses;
    }
}