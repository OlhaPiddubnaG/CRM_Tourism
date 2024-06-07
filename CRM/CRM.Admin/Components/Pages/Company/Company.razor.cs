using CRM.Admin.Components.Dialogs.Company;
using CRM.Admin.Components.Dialogs.User;
using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.Data.UserDTO;
using CRM.Admin.Requests.CompanyRequests;
using CRM.Admin.Requests.UserRequests;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Company;

public partial class Company
{
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] ICompanyRequest CompanyRequest { get; set; } = default!;
    [Inject] IUserRequest UserRequest { get; set; } = default!;
    
    private DialogOptions dialogOptions = new() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
    private IEnumerable<CompanyDTO> _companyDTOs = new List<CompanyDTO>();
    private IEnumerable<CompanyDTO> _filteredCompanyDTOs = new List<CompanyDTO>();
    private IEnumerable<UserDTO> _userDTOs = new List<UserDTO>();
    private bool _loading = true;
    private Guid? _selectedCompanyId;

    private MudTable<CompanyDTO> _tableCompany = default!;
    private MudDataGrid<UserDTO> _tableUser = default!;
    
    protected override async Task OnInitializedAsync()
    {
        _userDTOs = await UserRequest.GetAllAsync();
        _companyDTOs = await CompanyRequest.GetAllAsync();
        _filteredCompanyDTOs = _companyDTOs.Where(c => !c.IsDeleted && c.Name != "Admin");

    }
    
    private async Task CreateUser()
    {
        _loading = true;
        var dialogReference = await DialogService.ShowAsync<CreateUserDialog>("", dialogOptions);
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
            "Warning",
            "Deleting can not be undone!",
            yesText: "Delete", cancelText: "Cancel");

        if (result is not null)
            await UserRequest.DeleteAsync(id);

        _loading = false;
        await _tableUser.ReloadServerData();
    }
    
    private async Task DeleteCompany(Guid id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning",
            "Deleting can not be undone!",
            yesText: "Delete", cancelText: "Cancel");

        if (result is not null)
            await CompanyRequest.DeleteAsync(id);

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
        var filteredUsers = _userDTOs.Where(user => user.CompanyId == _selectedCompanyId.Value).ToList();
        _loading = false;
        var pagedData = filteredUsers.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new GridData<UserDTO>
        {
            Items = pagedData,
            TotalItems = filteredUsers.Count()
        };
    }

    private void OnCompanySelected(TableRowClickEventArgs<CompanyDTO> args)
    {
        _selectedCompanyId = args.Item.Id;
        _tableUser.ReloadServerData();
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