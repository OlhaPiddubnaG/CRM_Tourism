using CRM.Admin.Auth;
using CRM.Admin.Components.Dialogs.Reminder;
using CRM.Admin.Data.UserTasksDto;
using CRM.Admin.Requests.UserTasksRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Reminder;

public partial class Reminder
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IUserTasksRequest UserTasksRequest { get; set; } = null!;
    [Inject] private AuthState AuthState { get; set; }

    private readonly DialogOptions _dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        CloseButton = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small,
        FullWidth = true
    };

    private List<UserTasksDto> _tasksDtos = new();
    private DateTime? _selectedDate = DateTime.Today;
    private DateTime? _lastLoadedDate;
    private Guid _userId;

    protected override async Task OnInitializedAsync()
    {
        _userId = AuthState.UserId;
        await LoadTasksForSelectedDate();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender || (_selectedDate.HasValue && _selectedDate != _lastLoadedDate))
        {
            await LoadTasksForSelectedDate();
            _lastLoadedDate = _selectedDate;
        }
    }

    private async Task LoadTasksForSelectedDate()
    {
        if (_selectedDate.HasValue)
        {
            var requestParameters = new UserTasksRequestParameters
            {
                UserId = _userId,
                DateTime = _selectedDate
            };
            _tasksDtos = await UserTasksRequest.GetTasksByUserIdAndDateAsync(requestParameters);
            StateHasChanged();
        }
    }

    private async Task CreateReminder()
    {
        var parameters = new DialogParameters { { "Id", _userId } };
        _dialogOptions.MaxWidth = MaxWidth.Small;
        var dialogReference = await DialogService.ShowAsync<CreateReminderDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (!dialogResult.Canceled && dialogResult.Data is UserTasksDto newTasksDto)
        {
            _tasksDtos.Add(newTasksDto);
        }
    }  
    private async Task UpdateReminder(Guid taskId)
    {
        var parameters = new DialogParameters { { "Id", taskId } };
        _dialogOptions.MaxWidth = MaxWidth.Small;
        var dialogReference = await DialogService.ShowAsync<UpdateReminderDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (!dialogResult.Canceled && dialogResult.Data is UserTasksDto updatedTaskDto)
        {
            var index = _tasksDtos.FindIndex(t => t.Id == taskId);

            if (index != -1)
            {
                _tasksDtos[index] = updatedTaskDto;
            }
        }
    }
    
    private async Task DeleteReminder(Guid taskId)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Увага",
            "Ви впевнені, що хочете видалити вибраний об'єкт?",
            yesText: "Так", cancelText: "Ні");

        if (result is not null)
        {
            try
            {
                await UserTasksRequest.DeleteAsync(taskId);
                Snackbar.Add("Компанія успішно видалена", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Помилка видалення компанії: {ex.Message}", Severity.Error);
            }
        }
        await LoadTasksForSelectedDate();
    }

    private async Task AddStatus(Guid taskId)
    {
        var parameters = new DialogParameters { { "Id", taskId } };
        _dialogOptions.MaxWidth = MaxWidth.ExtraSmall;
        var dialogReference = await DialogService.ShowAsync<AddStatusForUserTaskDialog>("", parameters, _dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (!dialogResult.Canceled && dialogResult.Data is UserTasksDto updatedStatusTaskDto)
        {
            var index = _tasksDtos.FindIndex(t => t.Id == taskId);

            if (index != -1)
            {
                _tasksDtos[index] = updatedStatusTaskDto;
            }
        }
    }

    private async Task NavigateToAllReminders()
    {
        NavigationManager.NavigateTo($"/allReminders/{_userId}");
    }
}