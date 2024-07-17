using CRM.Admin.Data.CompanyDto;
using CRM.Admin.Data.UserDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.SuperAdminRequests;

public class SuperAdminRequest : ISuperAdminRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<SuperAdminRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/SuperAdmin";

    public SuperAdminRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<SuperAdminRequest> logger,
        ISnackbar snackbar)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateCompanyAsync(CompanyCreateDto companyCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/company", companyCreateDto);
            _logger.LogInformation("CreateCompany method executed successfully");
            var createdCompany = await response.Content.ReadFromJsonAsync<CompanyDto>();
            _snackbar.Add("Компанія успішно створена", Severity.Success);
            return createdCompany.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateCompany method");
            _snackbar.Add($"Помилка при створенні компанії: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<Guid> CreateUserAsync(UserCreateDto userCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/companyAdmin", userCreateDto);
            _logger.LogInformation("CreateUser method executed successfully");
            var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
            _snackbar.Add("Користувач успішно створений", Severity.Success);
            return createdUser.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateUser method");
            _snackbar.Add($"Помилка при створенні користувача: {ex.Message}", Severity.Error);
            throw;
        }
    }
}