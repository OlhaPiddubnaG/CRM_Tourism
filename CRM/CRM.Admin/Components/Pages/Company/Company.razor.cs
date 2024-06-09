using CRM.Admin.Components.Dialogs.Company;
using CRM.Admin.Components.Dialogs.User;
using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Data.UserDTO;
using CRM.Admin.Requests.CompanyRequests;
using CRM.Admin.Requests.UserRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Company;

public partial class Company
{
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] ISnackbar Snackbar { get; set; } = default!;
    [Inject] ICompanyRequest CompanyRequest { get; set; } = default!;
    [Inject] IUserRequest UserRequest { get; set; } = default!;

    private DialogOptions dialogOptions = new() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
    private IEnumerable<CompanyDTO> _companyDTOs = new List<CompanyDTO>();
    private IEnumerable<UserDTO> _userDTOs = new List<UserDTO>();
    private IEnumerable<UserDTO> _filteredUserDTOs = new List<UserDTO>();
    private bool _loading = true;
    private bool _isCreateUserButtonDisabled = true;
    private Guid? _selectedCompanyId;

    private MudTable<CompanyDTO> _tableCompany = default!;
    private MudDataGrid<UserDTO> _tableUser = default!;

    protected override async Task OnInitializedAsync()
    {
        _userDTOs = (await UserRequest.GetAllAsync()).Where(u => !u.IsDeleted && u.Name != "Admin").ToList();
        _companyDTOs = (await CompanyRequest.GetAllAsync()).Where(c => !c.IsDeleted && c.Name != "Admin").ToList();
        UpdateCreateUserButtonState();
    }

    private async Task CreateUser(Guid selectedCompanyId)
    {
        _loading = true;
        var parameters = new DialogParameters { { "Id", selectedCompanyId } };
        var dialogReference = await DialogService.ShowAsync<CreateUserDialog>("", parameters, dialogOptions);
        var dialogResult = await dialogReference.Result;

        _loading = false;
        if (dialogResult.Canceled)
            return;

        await _tableUser.ReloadServerData();
    }

    private async Task CreateCompany()
    {
        _loading = true;
        var dialogReference = await DialogService.ShowAsync<CreateCompanyDialog>("", dialogOptions);
        var dialogResult = await dialogReference.Result;

        _loading = false;
        if (dialogResult.Canceled)
            return;

        await _tableUser.ReloadServerData();
    }

    private async Task UpdateUser(Guid id)
    {
        _loading = true;
        var parameters = new DialogParameters { { "Id", id } };
        var dialogReference = await DialogService.ShowAsync<UpdateUserDialog>("", parameters, dialogOptions);
        var dialogResult = await dialogReference.Result;

        _loading = false;
        if (dialogResult.Canceled)
            return;

        await _tableUser.ReloadServerData();
    }

    private async Task UpdateCompany(Guid id)
    {
        _loading = true;
        var parameters = new DialogParameters { { "Id", id } };
        var dialogReference = await DialogService.ShowAsync<UpdateCompanyDialog>("", parameters, dialogOptions);
        var dialogResult = await dialogReference.Result;

        _loading = false;
        if (dialogResult.Canceled)
            return;

        await _tableUser.ReloadServerData();
    }

    private async Task DeleteUser(Guid id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Увага",
            "Ви впевнені, що хочете видалити вибраний обєкт?",
            yesText: "Так", cancelText: "Ні");

        if (result is not null && result == true)
        {
            await UserRequest.DeleteAsync(id);
            Snackbar.Add("Користувач успішно видалений", Severity.Success);
        }

        _loading = false;
        await _tableUser.ReloadServerData();
    }

    private async Task DeleteCompany(Guid id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Увага",
            "Ви впевнені, що хочете видалити вибраний обєкт?",
            yesText: "Так", cancelText: "Ні");

        if (result is not null)
        {
            await CompanyRequest.DeleteAsync(id);
            Snackbar.Add("Компанія успішно видалена", Severity.Success);
        }

        _loading = false;
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

        _loading = true;
        _filteredUserDTOs = _userDTOs.Where(user => user.CompanyId == _selectedCompanyId.Value).ToList();
        
        _loading = false;
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
        if (_selectedCompanyId == null)
        {
            _isCreateUserButtonDisabled = true;
        }
        else
        {
            _isCreateUserButtonDisabled = false;
        }
    }

    private string SelectedRowClassFunc(CompanyDTO company, int rowNumber)
    {
        if (_selectedCompanyId.HasValue && _selectedCompanyId.Value == company.Id)
        {
            return "selected";
        }

        return string.Empty;
    }
}