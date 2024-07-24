using CRM.Admin.Data.CompanyDto;
using CRM.Admin.Data.UserDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.SuperAdminRequests;

public class SuperAdminRequest : ISuperAdminRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<SuperAdminRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/SuperAdmin";

    public SuperAdminRequest(IHttpRequests httpRequests, ILogger<SuperAdminRequest> logger,
        ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateCompanyAsync(CompanyCreateDto dto)
    {
        try
        {
            var createdCompany = await _httpRequests.SendPostRequestAsync<CompanyDto>($"{RequestUri}/company", dto);
            _logger.LogInformation("CreateCompany method executed successfully");
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

    public async Task<Guid> CreateUserAsync(UserCreateDto dto)
    {
        try
        {
            var createdUser = await _httpRequests.SendPostRequestAsync<UserDto>($"{RequestUri}/companyAdmin", dto);
            _logger.LogInformation("CreateUser method executed successfully");
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