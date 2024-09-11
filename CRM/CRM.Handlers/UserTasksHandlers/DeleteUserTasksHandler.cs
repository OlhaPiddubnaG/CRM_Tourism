using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserTasksHandlers;

public class DeleteUserTasksHandler : IRequestHandler<DeleteCommand<UserTasks>, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteUserTasksHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(DeleteCommand<UserTasks> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var userTasks = await _context.UserTasks
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (userTasks == null)
        {
            throw new NotFoundException(typeof(UserTasks), request.Id);
        }

        if (companyId != userTasks.User.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this task.");
        }

        if (userTasks.IsDeleted)
        {
            throw new InvalidOperationException($"Task with ID {request.Id} is already deleted.");
        }

        userTasks.IsDeleted = true;
        userTasks.DeletedAt = DateTime.UtcNow;
        userTasks.DeletedUserId = _currentUser.GetUserId();

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
            Message = "Successfully deleted."
        };
    }
}