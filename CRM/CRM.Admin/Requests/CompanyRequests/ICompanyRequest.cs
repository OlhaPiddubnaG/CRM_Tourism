using CRM.Admin.Data.CompanyDTO;

namespace CRM.Admin.Requests.CompanyRequests;

public interface ICompanyRequest
{
    Task<List<CompanyDTO>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : ICompanyDTO;
    Task<bool> UpdateAsync(CompanyUpdateDTO categotyUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}