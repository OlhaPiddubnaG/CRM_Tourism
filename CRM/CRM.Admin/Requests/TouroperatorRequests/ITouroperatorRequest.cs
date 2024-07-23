using CRM.Admin.Data.TouroperatorDto;

namespace CRM.Admin.Requests.TouroperatorRequests;

public interface ITouroperatorRequest
{
    Task<Guid> CreateAsync(TouroperatorCreateDto touroperatorCreateDto);
    Task<List<TouroperatorDto>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : ITouroperatorDto;
    Task<bool> UpdateAsync(TouroperatorUpdateDto touroperatorUpdateDto);
    Task<bool> DeleteAsync(Guid id);
}