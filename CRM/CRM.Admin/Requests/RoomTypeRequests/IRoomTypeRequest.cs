using CRM.Admin.Data.RoomTypeDto;

namespace CRM.Admin.Requests.RoomTypeRequests;

public interface IRoomTypeRequest
{
    Task<Guid> CreateAsync(RoomTypeCreateDto dto);
    Task<List<RoomTypeDto>> GetAllAsync();
    Task<bool> UpdateAsync(RoomTypeUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}