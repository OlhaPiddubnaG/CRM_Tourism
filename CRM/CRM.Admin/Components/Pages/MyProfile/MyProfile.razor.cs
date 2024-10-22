using CRM.Admin.Auth;
using CRM.Admin.Data.UserDto;
using CRM.Admin.Requests.UserRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.MyProfile;

public partial class MyProfile
{
    [Inject] private IUserRequest UserRequest { get; set; } = null!;
    [Inject] private ISnackbar? Snackbar { get; set; }
    [Inject] private AuthState AuthState { get; set; }

    private UserUpdateDto _userUpdateDto = new();
    private string _email;
    private Guid _userId;

    protected override async Task OnInitializedAsync()
    {
        _userId = AuthState.UserId;
        _userUpdateDto = await UserRequest.GetByIdAsync(_userId);
        _email = (_userUpdateDto.Email).ToLower();
    }

    private void ShowEmailWarning()
    {
        Snackbar.Add("Email не можна редагувати.", Severity.Warning);
    }

    private async Task Update()
    {
        _userUpdateDto.RoleTypes ??= new List<RoleType>();
        var result = await UserRequest.UpdateAsync(_userUpdateDto);
        if (result)
        {
            Snackbar.Add("Дані  успішно оновлені", Severity.Success);
        }
        else
        {
            Snackbar.Add($"Помилка при редагуванні", Severity.Error);
        }
    }
}