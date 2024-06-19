using CRM.Admin.Data.PassportInfoDTO;

namespace CRM.Admin.Requests.PassportInfoRequests;

public interface IPassportInfoRequest
{
    Task<Guid> CreateAsync(PassportInfoCreateDTO passportInfoCreateDTO);
    Task<List<PassportInfoDTO>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IPassportInfoDTO;
    Task<bool> UpdateAsync(PassportInfoUpdateDTO passportInfoUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}