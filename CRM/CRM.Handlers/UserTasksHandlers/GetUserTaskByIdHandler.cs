using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.UserTasks;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserTasksHandlers;

public class GetUserTaskByIdHandler : IRequestHandler<GetByIdRequest<UserTasksResponse>,
    UserTasksResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetUserTaskByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<UserTasksResponse> Handle(GetByIdRequest<UserTasksResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var userTask = await _context.UserTasks
            .FirstOrDefaultAsync(s => s.Id == request.Id &&
                                      s.User.CompanyId == companyId &&
                                      !s.IsDeleted);

        if (userTask == null)
        {
            throw new NotFoundException(typeof(UserTasks), request.Id);
        }

        var userTaskResponse = _mapper.Map<UserTasksResponse>(userTask);

        return userTaskResponse;
    }
}