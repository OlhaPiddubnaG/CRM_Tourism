using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.UserTasks;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserTasksHandlers;

public class CreateUserTasksHandler : IRequestHandler<CreateUserTasksCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateUserTasksHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateUserTasksCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId &&
                                      u.CompanyId == companyId &&
                                      !u.IsDeleted, cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException(
                "User not found or user is not authorized to create tasks from a different company.");
        }

        var userTask = _mapper.Map<UserTasks>(request);
        
        userTask.CreatedAt = DateTime.UtcNow;
        userTask.CreatedUserId = _currentUser.GetUserId();
        
        _context.UserTasks.Add(userTask);
        
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(UserTasks), ex);
        }

        return new CreatedResponse(userTask.Id);
    }
}