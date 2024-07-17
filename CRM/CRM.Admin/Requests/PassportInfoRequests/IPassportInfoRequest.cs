using CRM.Admin.Data.PassportInfoDto;

namespace CRM.Admin.Requests.PassportInfoRequests;

public interface IPassportInfoRequest
{
    Task<Guid> CreateAsync(PassportInfoCreateDto passportInfoCreateDTO);
    Task<List<PassportInfoDto>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IPassportInfoDto;
    Task<List<PassportInfoDto>> GetByClientPrivateDataIdAsync(Guid clientPrivateDataId);
    Task<bool> UpdateAsync(PassportInfoUpdateDto passportInfoUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}