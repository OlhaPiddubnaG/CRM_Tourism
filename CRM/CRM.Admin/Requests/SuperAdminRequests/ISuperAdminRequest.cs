using CRM.Admin.Data.CompanyDto;
using CRM.Admin.Data.UserDto;

namespace CRM.Admin.Requests.SuperAdminRequests;

public interface ISuperAdminRequest
{
    Task CreateCompanyAsync(CompanyCreateDto categoryCreateDTO);
    Task CreateUserAsync(UserCreateDto userCreateDTO);
}