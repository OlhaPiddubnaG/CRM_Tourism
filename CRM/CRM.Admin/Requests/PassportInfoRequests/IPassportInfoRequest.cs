using CRM.Admin.Data.PassportInfoDto;

namespace CRM.Admin.Requests.PassportInfoRequests;

public interface IPassportInfoRequest
{
    Task<Guid> CreateAsync(PassportInfoCreateDto dto);
    Task<List<PassportInfoDto>> GetAllAsync();
    Task<PassportInfoUpdateDto> GetByIdAsync(Guid id);
    Task<List<PassportInfoDto>> GetByClientPrivateDataIdAsync(Guid clientPrivateDataId);
    Task<bool> UpdateAsync(PassportInfoUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}