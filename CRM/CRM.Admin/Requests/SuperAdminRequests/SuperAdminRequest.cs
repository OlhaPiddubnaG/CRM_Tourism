using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Data.UserDTO;
using CRM.Admin.HttpRequests;

namespace CRM.Admin.Requests.SuperAdminRequests;

public class SuperAdminRequest : ISuperAdminRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private const string RequestUri = "api/SuperAdmin";
    
    public SuperAdminRequest(IHttpCrmApiRequests httpCrmApiRequests)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
    }
    
    public async Task CreateCompany(CompanyCreateDTO categoryCreateDTO)
        => await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/company", categoryCreateDTO);
    
    public async Task CreateCompanyAdmin(UserCreateDTO userCreateDTO)
        => await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/companyAdmin", userCreateDTO);
}