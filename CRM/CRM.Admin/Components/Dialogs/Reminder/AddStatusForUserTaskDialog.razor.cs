using CRM.Admin.Data.UserTasksDto;
using CRM.Admin.Requests.UserTasksRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Reminder;

public partial class AddStatusForUserTaskDialog
{
    [Inject] private IUserTasksRequest UserTasksRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }

    private UserTasksUpdateDto _userTasksUpdateDto { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        _userTasksUpdateDto = await UserTasksRequest.GetByIdAsync(Id);
    }

    private async Task Update()
    {
        var result = await UserTasksRequest.UpdateAsync(_userTasksUpdateDto);
        if (result)
        {
            Snackbar.Add("Статус змінено", Severity.Success);
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
            Snackbar.Add("Помилка при зміні статусу", Severity.Error);
        }
    }

    void Cancel() => MudDialog.Cancel();
}
