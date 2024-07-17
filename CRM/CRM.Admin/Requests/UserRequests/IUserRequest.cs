using CRM.Admin.Data.UserDto;

namespace CRM.Admin.Requests.UserRequests;

public interface IUserRequest
{
    Task<List<UserDto>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IUserDto;
    Task<Guid> CreateAsync(UserCreateDto userCreateDTO);
    Task<bool> UpdateAsync(UserUpdateDto userUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}