using CRM.Admin.Data.UserDto;

namespace CRM.Admin.Requests.UserRequests;

public interface IUserRequest
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserUpdateDto> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(UserCreateDto dto);
    Task<bool> UpdateAsync(UserUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}