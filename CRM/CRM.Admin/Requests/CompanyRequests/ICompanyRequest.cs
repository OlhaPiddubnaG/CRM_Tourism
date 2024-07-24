using CRM.Admin.Data.CompanyDto;

namespace CRM.Admin.Requests.CompanyRequests;

public interface ICompanyRequest
{
    Task<List<CompanyDto>> GetAllAsync();
    Task<CompanyUpdateDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(CompanyUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}