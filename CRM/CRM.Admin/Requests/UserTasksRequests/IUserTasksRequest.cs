using CRM.Admin.Data.UserTasksDto;

namespace CRM.Admin.Requests.UserTasksRequests;

public interface IUserTasksRequest
{
    Task<Guid> CreateAsync(UserTasksCreateDto dto);
    Task<List<UserTasksDto>> GetTasksByUserIdAsync(Guid userId);
    Task<List<UserTasksDto>> GetTasksByUserIdAndDateAsync(UserTasksRequestParameters parameters);
    Task<UserTasksUpdateDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(UserTasksUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}