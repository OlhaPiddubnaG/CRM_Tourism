using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.UserTasks;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserTasksHandlers;

public class UpdateUserTasksHandler : IRequestHandler<UpdateUserTasksCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateUserTasksHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateUserTasksCommand request,
        CancellationToken cancellationToken)
    {
        var existingUserTasks = await _context.UserTasks
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (existingUserTasks == null)
        {
            throw new NotFoundException(typeof(UserTasks), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (existingUserTasks.User.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update task.");
        }
        
        existingUserTasks.DateTime = request.DateTime;
        existingUserTasks.Description = request.Description;
        existingUserTasks.TaskStatus = request.TaskStatus;
        existingUserTasks.UpdatedAt = DateTime.UtcNow;
        existingUserTasks.UpdatedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(UserTasks), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}