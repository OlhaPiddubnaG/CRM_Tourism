using CRM.Admin.Data.UserTasksDto;
using CRM.Admin.Requests.UserTasksRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Reminder;

public partial class UpdateReminderDialog
{
    [Inject] private IUserTasksRequest UserTasksRequest { get; set; } = null!;
    [Inject] private ISnackbar? Snackbar { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private UserTasksUpdateDto _userTasksUpdateDto { get; set; } = new();
    private DateTime? _date;
    private TimeSpan? _time;

    protected override async Task OnInitializedAsync()
    {
        _userTasksUpdateDto = await UserTasksRequest.GetByIdAsync(Id);
        _date = _userTasksUpdateDto.DateTime.Date;
        _time = _userTasksUpdateDto.DateTime.TimeOfDay;
    }

    private async Task Update()
    {
        try
        {
            _userTasksUpdateDto.UserId = Id;
            if (_date.HasValue && _time.HasValue)
            {
                var localDateTime = _date.Value.Date.Add(_time.Value);
                _userTasksUpdateDto.DateTime = localDateTime.ToUniversalTime();
            }
            else
            {
                Snackbar.Add("Please select a valid date and time", Severity.Error);
                return;
            }

            var result = await UserTasksRequest.UpdateAsync(_userTasksUpdateDto);
            if (result)
            {
                Snackbar.Add("Дані нагадування успішно оновлені", Severity.Success);
                MudDialog.Close(DialogResult.Ok(new UserTasksDto
                {
                    Id = _userTasksUpdateDto.Id,
                    Description = _userTasksUpdateDto.Description,
                    DateTime = _userTasksUpdateDto.DateTime,
                    TaskStatus = _userTasksUpdateDto.TaskStatus
                }));
            }
            else
            {
                Snackbar.Add($"Помилка при редагуванні нагадування", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add($"Помилка при редагуванні нагадування", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}