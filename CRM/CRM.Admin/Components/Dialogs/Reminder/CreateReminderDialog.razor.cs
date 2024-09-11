using CRM.Admin.Data.UserTasksDto;
using CRM.Admin.Requests.UserTasksRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Reminder;

public partial class CreateReminderDialog
{
    [Inject] private IUserTasksRequest UserTasksRequest { get; set; } = null!;
    [Inject] private ISnackbar? Snackbar { get; set; }

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private UserTasksCreateDto _userTasksCreateDto { get; set; } = new();
    private DateTime? _date = DateTime.UtcNow;
    private TimeSpan? _time = new TimeSpan(09, 00, 00);
    private DateTime _dateTime; 

    private async Task Create()
    {
        try
        {
            _userTasksCreateDto.UserId = Id;
            if (_date.HasValue && _time.HasValue)
            {
                var localDateTime = _date.Value.Date.Add(_time.Value);
                _dateTime = localDateTime.ToUniversalTime();
                _userTasksCreateDto.DateTime = _dateTime;
            }
            else
            {
                Snackbar.Add("Please select a valid date and time", Severity.Error);
                return;
            }

            var taskId = await UserTasksRequest.CreateAsync(_userTasksCreateDto);
            if (taskId != Guid.Empty)
            {
                Snackbar.Add("Додано нагадування", Severity.Success);
                MudDialog.Close(DialogResult.Ok(new UserTasksDto
                    { Id = taskId, Description = _userTasksCreateDto.Description, DateTime = _dateTime, TaskStatus = _userTasksCreateDto.TaskStatus}));
            }
            else
            {
                Snackbar.Add($"Помилка при додаванні нагадування", Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add($"Помилка при додаванні нагадування", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}