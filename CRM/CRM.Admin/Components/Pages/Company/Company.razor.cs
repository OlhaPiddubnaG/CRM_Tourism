using CRM.Admin.Components.Dialogs.Company;
using CRM.Admin.Components.Dialogs.User;
using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Data.UserDTO;
using CRM.Admin.Requests.CompanyRequests;
using CRM.Admin.Requests.UserRequests;
using CRM.Domain.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Company;

public partial class Company
{
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] ISnackbar Snackbar { get; set; } = default!;
    [Inject] ICompanyRequest CompanyRequest { get; set; } = default!;
    [Inject] IUserRequest UserRequest { get; set; } = default!;
    
    private DialogOptions dialogOptions = new() 
    {   
        CloseOnEscapeKey = true, 
        CloseButton = true, 
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small, 
        FullWidth = true 
    };
    private IEnumerable<CompanyDTO> _companyDTOs = new List<CompanyDTO>();
    private IEnumerable<UserDTO> _userDTOs = new List<UserDTO>();
    private IEnumerable<UserDTO> _filteredUserDTOs = new List<UserDTO>();
    private bool _isCreateUserButtonDisabled = true;
    private Guid? _selectedCompanyId;

    private MudTable<CompanyDTO> _tableCompany = default!;
    private MudDataGrid<UserDTO> _tableUser = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        _userDTOs = (await UserRequest.GetAllAsync()).Where(u => !u.IsDeleted && u.Name != Constants.DefaultAdminUserName).ToList();
        _companyDTOs = (await CompanyRequest.GetAllAsync()).Where(c => !c.IsDeleted && c.Name != Constants.DefaultCompanyAdminName).ToList();
        StateHasChanged();
    }

    private async Task CreateUser()
    {
        var parameters = new DialogParameters { { "Id", _selectedCompanyId } };
        var dialogReference = await DialogService.ShowAsync<CreateUserDialog>("", parameters, dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await LoadDataAsync();
        await _tableUser.ReloadServerData();
    }

    private async Task CreateCompany()
    {
        var dialogReference = await DialogService.ShowAsync<CreateCompanyDialog>("", dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await LoadDataAsync();
        await _tableCompany.ReloadServerData();
    }

    private async Task UpdateUser(Guid id)
    {
        var parameters = new DialogParameters { { "Id", id } };
        var dialogReference = await DialogService.ShowAsync<UpdateUserDialog>("", parameters, dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await LoadDataAsync();
        await _tableUser.ReloadServerData();
    }

    private async Task UpdateCompany(Guid id)
    {
        var parameters = new DialogParameters { { "Id", id } };
        var dialogReference = await DialogService.ShowAsync<UpdateCompanyDialog>("", parameters, dialogOptions);
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

    private async Task<GridData<UserDTO>> SetTableDataUserAsync(GridState<UserDTO> state)
    {
        if (_selectedCompanyId == null)
        {
            return new GridData<UserDTO>
            {
                Items = new List<UserDTO>(),
                TotalItems = 0
            };
        }

        _filteredUserDTOs = _userDTOs.Where(user => user.CompanyId == _selectedCompanyId.Value).ToList();
        var pagedData = _filteredUserDTOs.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new GridData<UserDTO>
        {
            Items = pagedData,
            TotalItems = _filteredUserDTOs.Count()
        };
    }

    private void OnCompanySelected(TableRowClickEventArgs<CompanyDTO> args)
    {
        _selectedCompanyId = args.Item.Id;
        _tableUser.ReloadServerData();
        UpdateCreateUserButtonState();
    }

    private void UpdateCreateUserButtonState()
    {
        _isCreateUserButtonDisabled = _selectedCompanyId == null;
    }

    private string SelectedRowClassFunc(CompanyDTO company, int rowNumber)
    {
        return _selectedCompanyId.HasValue && _selectedCompanyId.Value == company.Id ? "selected-row" : string.Empty;
    }
}