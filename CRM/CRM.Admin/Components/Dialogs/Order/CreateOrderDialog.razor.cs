using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Dialogs.Order;

public partial class CreateOrderDialog
{
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid Id { get; set; }
    
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    private void Cancel() => MudDialog.Cancel();
}