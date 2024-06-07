using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Data.UserDTO;

namespace CRM.Admin.Requests.SuperAdminRequests;

public interface ISuperAdminRequest
{
    Task CreateCompany(CompanyCreateDTO categoryCreateDTO);
    Task CreateCompanyAdmin(UserCreateDTO userCreateDTO);
}