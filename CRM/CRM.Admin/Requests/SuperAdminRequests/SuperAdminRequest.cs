using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Data.UserDTO;
using CRM.Admin.HttpRequests;

namespace CRM.Admin.Requests.SuperAdminRequests;

public class SuperAdminRequest : ISuperAdminRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<SuperAdminRequest> _logger;
    private const string RequestUri = "api/SuperAdmin";

    public SuperAdminRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<SuperAdminRequest> logger)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
    }

    public async Task CreateCompany(CompanyCreateDTO categoryCreateDTO)
    {
        try
        {
            await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/company", categoryCreateDTO);
            _logger.LogInformation("CreateCompany method executed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateCompany method");
            throw;
        }
    }

    public async Task CreateUser(UserCreateDTO userCreateDTO)
    {
        try
        {
            await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/companyAdmin", userCreateDTO);
            _logger.LogInformation("CreateUser method executed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateUser method");
            throw;
        }
    }
}