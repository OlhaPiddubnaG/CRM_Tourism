using CRM.Admin.Components.Dialogs.Client;
using CRM.Admin.Components.Dialogs.Order;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Layout;

public partial class AppBar
{
    [Inject] IDialogService DialogService { get; set; } = default!;
    private DialogOptions dialogOptions = new()
    {
        CloseOnEscapeKey = true,
        CloseButton = true,
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.Small,
        FullWidth = true
    };
    bool disabled = false;

    private async Task NewClient()
    {
        var dialogReference = await DialogService.ShowAsync<CreateClientDialog>("", dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    }

    private async Task NewOrder()
    {
        var dialogReference = await DialogService.ShowAsync<CreateOrderDialog>("", dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;
    }
}