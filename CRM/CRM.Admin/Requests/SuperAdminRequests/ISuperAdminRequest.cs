using CRM.Admin.Data.CompanyDto;
using CRM.Admin.Data.UserDto;

namespace CRM.Admin.Requests.SuperAdminRequests;

public interface ISuperAdminRequest
{
    Task<Guid> CreateCompanyAsync(CompanyCreateDto categoryCreateDTO);
    Task<Guid> CreateUserAsync(UserCreateDto userCreateDTO);
}