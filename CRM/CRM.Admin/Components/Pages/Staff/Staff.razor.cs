using CRM.Admin.Components.Dialogs.User;
using CRM.Admin.Data.UserDto;
using CRM.Admin.Requests.UserRequests;
using CRM.Domain.Constants;
using CRM.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Staff;

public partial class Staff
{
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
    [Inject] IDialogService DialogService { get; set; } = default!;
    [Inject] ISnackbar Snackbar { get; set; } = default!;
    [Inject] IUserRequest UserRequest { get; set; } = default!;
    
    private DialogOptions dialogOptions = new() 
    {   
        CloseOnEscapeKey = true, 
        CloseButton = true, 
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small, 
        FullWidth = true 
    };
    private IEnumerable<UserDto> _userDTOs = new List<UserDto>();
    private MudDataGrid<UserDto> _tableUser = default!;
    public Guid CompanyId;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var companyIdClaim = authState.User.FindFirst(c => c.Type == CustomClaimTypes.CompanyId);
        if (companyIdClaim != null && Guid.TryParse(companyIdClaim.Value, out var companyId))
        {
            CompanyId = companyId;
        }
    }

    private async Task CreateUser()
    {
        var parameters = new DialogParameters { { "Id", CompanyId } };
        var dialogReference = await DialogService.ShowAsync<CreateStaffDialog>("", parameters, dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await _tableUser.ReloadServerData();
    }
    
    private async Task UpdateUser(Guid id)
    {
        var parameters = new DialogParameters { { "Id", id } };
        var dialogReference = await DialogService.ShowAsync<UpdateUserDialog>("", parameters, dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await _tableUser.ReloadServerData();
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

        await _tableUser.ReloadServerData();
    }
    
    private async Task<GridData<UserDto>> SetTableDataUserAsync(GridState<UserDto> state)
    {
        // if (_selectedCompanyId == null)
        // {
        //     return new GridData<UserDTO>
        //     {
        //         Items = new List<UserDTO>(),
        //         TotalItems = 0
        //     };
        // }

        _userDTOs = (await UserRequest.GetAllAsync()).Where(u => !u.IsDeleted && u.Name != Constants.DefaultCompanyAdminUserName).ToList();
        var pagedData = _userDTOs.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
        return new GridData<UserDto>
        {
            Items = pagedData,
            TotalItems = _userDTOs.Count()
        };
    }
}