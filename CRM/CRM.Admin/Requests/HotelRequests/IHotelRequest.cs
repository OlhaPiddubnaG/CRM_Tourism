using CRM.Admin.Data.HotelDto;

namespace CRM.Admin.Requests.HotelRequests;

public interface IHotelRequest
{
    Task<Guid> CreateAsync(HotelCreateDto dto);
    Task<List<HotelDto>> GetFiltredDataAsync(string searchString);
    Task<List<HotelDto>> GetAllAsync();
    Task<HotelDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(HotelUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}