using CRM.Admin.Data.UserDTO;

namespace CRM.Admin.Requests.UserRequests;

public interface IUserRequest
{
    Task<List<UserDTO>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IUserDTO;
    Task CreateAsync(UserCreateDTO userCreateDTO);
    Task<bool> UpdateAsync(UserUpdateDTO userUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}