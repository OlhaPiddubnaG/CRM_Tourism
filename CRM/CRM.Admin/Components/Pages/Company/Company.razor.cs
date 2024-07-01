using CRM.Admin.Components.Dialogs.Company;
using CRM.Admin.Components.Dialogs.User;
using CRM.Admin.Data.CompanyDto;
using CRM.Admin.Data.UserDto;
using CRM.Admin.Requests.CompanyRequests;
using CRM.Admin.Requests.UserRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Company;

public partial class Company
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private ICompanyRequest CompanyRequest { get; set; } = null!;
    [Inject] private IUserRequest UserRequest { get; set; } = null!;
    
    private readonly DialogOptions _dialogOptions = new() 
    {   
        CloseOnEscapeKey = true, 
        CloseButton = true, 
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small, 
        FullWidth = true 
    };
    private IEnumerable<CompanyDto> _companyDtos = new List<CompanyDto>();
    private IEnumerable<UserDto> _userDtos = new List<UserDto>();
    private IEnumerable<UserDto> _filteredUserDtos = new List<UserDto>();
    private bool _isCreateUserButtonDisabled = true;
    private Guid? _selectedCompanyId;

    private MudTable<CompanyDto> _tableCompany = null!;
    private MudDataGrid<UserDto> _tableUser = null!;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        _userDtos = await UserRequest.GetAllAsync();
        _companyDtos = await CompanyRequest.GetAllAsync();
        StateHasChanged();
    }

    private async Task CreateUser()
    {
        var parameters = new DialogParameters { { "Id", _selectedCompanyId } };
        var dialogReference = await DialogService.ShowAsync<CreateUserDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await LoadDataAsync();
        await _tableUser.ReloadServerData();
    }

    private async Task CreateCompany()
    {
        var dialogReference = await DialogService.ShowAsync<CreateCompanyDialog>("", _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await LoadDataAsync();
        await _tableCompany.ReloadServerData();
    }

    private async Task UpdateUser(Guid id)
    {
        var parameters = new DialogParameters { { "Id", id } };
        var dialogReference = await DialogService.ShowAsync<UpdateUserDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await LoadDataAsync();
        await _tableUser.ReloadServerData();
    }

    private async Task UpdateCompany(Guid id)
    {
        var parameters = new DialogParameters { { "Id", id } };
        var dialogReference = await DialogService.ShowAsync<UpdateCompanyDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await LoadDataAsync();
        await _tableCompany.ReloadServerData();
    }

    private async Task DeleteUser(Guid id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Увага",
            "Ви впевнені, що хочете видалити вибраний об'єкт?",
            yesText: "Так", cancelText: "Ні");

        if (result is not null && result == true)
        {
            try
            {
                await UserRequest.DeleteAsync(id);
                Snackbar.Add("Користувач успішно видалений", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Помилка видалення користувача: {ex.Message}", Severity.Error);
            }
        }

        await LoadDataAsync();
        await _tableUser.ReloadServerData();
    }

    private async Task DeleteCompany(Guid id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Увага",
            "Ви впевнені, що хочете видалити вибраний об'єкт?",
            yesText: "Так", cancelText: "Ні");

        if (result is not null)
        {
            try
            {
                await CompanyRequest.DeleteAsync(id);
                Snackbar.Add("Компанія успішно видалена", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Помилка видалення компанії: {ex.Message}", Severity.Error);
            }
        }

        await LoadDataAsync();
        await _tableCompany.ReloadServerData();
    }

    private async Task<GridData<UserDto>> SetTableDataUserAsync(GridState<UserDto> state)
    {
        if (_selectedCompanyId == null)
        {
            return new GridData<UserDto>
            {
                Items = new List<UserDto>(),
                TotalItems = 0
            };
        }

        _filteredUserDtos = _userDtos.Where(user => user.CompanyId == _selectedCompanyId.Value).ToList();
        var pagedData = _filteredUserDtos.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new GridData<UserDto>
        {
            Items = pagedData,
            TotalItems = _filteredUserDtos.Count()
        };
    }

    private void OnCompanySelected(TableRowClickEventArgs<CompanyDto> args)
    {
        _selectedCompanyId = args.Item.Id;
        _tableUser.ReloadServerData();
        UpdateCreateUserButtonState();
    }

    private void UpdateCreateUserButtonState()
    {
        _isCreateUserButtonDisabled = _selectedCompanyId == null;
    }

    private string SelectedRowClassFunc(CompanyDto company, int rowNumber)
    {
        return _selectedCompanyId.HasValue && _selectedCompanyId.Value == company.Id ? "selected-row" : string.Empty;
    }
}