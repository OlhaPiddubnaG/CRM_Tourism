using CRM.Admin.Data.MealsDto;

namespace CRM.Admin.Requests.MealsRequests;

public interface IMealsRequest
{
    Task<Guid> CreateAsync(MealsCreateDto dto);
    Task<List<MealsDto>> GetAllAsync();
    Task<MealsDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(MealsUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}