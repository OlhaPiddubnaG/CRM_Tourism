using CRM.Admin.Data.CompanyDto;

namespace CRM.Admin.Requests.CompanyRequests;

public interface ICompanyRequest
{
    Task<List<CompanyDto>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : ICompanyDto;
    Task<bool> UpdateAsync(CompanyUpdateDto categotyUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}