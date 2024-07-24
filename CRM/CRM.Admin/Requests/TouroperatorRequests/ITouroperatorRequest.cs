using CRM.Admin.Data.TouroperatorDto;

namespace CRM.Admin.Requests.TouroperatorRequests;

public interface ITouroperatorRequest
{
    Task<Guid> CreateAsync(TouroperatorCreateDto dto);
    Task<List<TouroperatorDto>> GetAllAsync();
    Task<TouroperatorDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(TouroperatorUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}