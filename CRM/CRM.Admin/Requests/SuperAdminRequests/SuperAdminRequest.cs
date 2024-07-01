using CRM.Admin.Data.CompanyDto;
using CRM.Admin.Data.UserDto;
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

    public async Task CreateCompanyAsync(CompanyCreateDto categoryCreateDto)
    {
        try
        {
            await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/company", categoryCreateDto);
            _logger.LogInformation("CreateCompany method executed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateCompany method");
            throw;
        }
    }

    public async Task CreateUserAsync(UserCreateDto userCreateDto)
    {
        try
        {
            await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/companyAdmin", userCreateDto);
            _logger.LogInformation("CreateUser method executed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateUser method");
            throw;
        }
    }
}