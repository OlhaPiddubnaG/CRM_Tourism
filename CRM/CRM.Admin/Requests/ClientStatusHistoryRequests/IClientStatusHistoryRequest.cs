using CRM.Admin.Data.ClientStatusHistoryDto;

namespace CRM.Admin.Requests.ClientStatusHistoryRequests;

public interface IClientStatusHistoryRequest
{
    Task<List<ClientStatusHistoryDto>> GetAllAsync(Guid clientId);
}