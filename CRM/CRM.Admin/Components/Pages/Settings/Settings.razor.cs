using System.Security.Claims;
using CRM.Admin.Auth;
using CRM.Admin.Components.Dialogs.User;
using CRM.Admin.Data.CompanyDto;
using CRM.Admin.Requests.CompanyRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Settings;

public partial class Settings
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private CustomAuthenticationStateProvider AuthenticationStateProvider  {get; set; }
    [Inject] private ICompanyRequest CompanyRequest { get; set; } = null!;
    [Inject] private AuthState AuthState { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    private readonly DialogOptions _dialogOptions = new() 
    {   
        CloseOnEscapeKey = true, 
        CloseButton = true, 
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small, 
        FullWidth = true 
    };
    
    private CompanyUpdateDto _сompanyUpdateDto { get; set; } = new();
    private ClaimsPrincipal _user;
    private Guid _companyId;
    private bool _isCompanyAdmin;
    
    protected override async Task OnInitializedAsync()
    {
        _companyId =  AuthState.CompanyId;
        _сompanyUpdateDto = await CompanyRequest.GetByIdAsync(_companyId);
        if (_сompanyUpdateDto.Id != Guid.Empty)
        {
            Snackbar.Add("Дані компанії завантажено", Severity.Success);
        }
        else
        {
            Snackbar.Add($"Помилка завантаження компанії", Severity.Error);
        }
        
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        _user = authState.User;

        if (_user.Identity.IsAuthenticated)
        {
            _isCompanyAdmin = _user.IsInRole("CompanyAdmin");
        }
    }
    
    private async Task CreateUser()
    {
        var parameters = new DialogParameters { { "Id", _companyId } };
        var dialogReference = await DialogService.ShowAsync<CreateStaffDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    }
    
    private async Task Update()
    {
        var result = await CompanyRequest.UpdateAsync(_сompanyUpdateDto);
        if (result)
        {
            Snackbar.Add("Назву компанії змінено", Severity.Success);
        }
        else
        {
            Snackbar.Add($"Помилка при внесенні змін в назву компанії", Severity.Error);
        }
    }
}