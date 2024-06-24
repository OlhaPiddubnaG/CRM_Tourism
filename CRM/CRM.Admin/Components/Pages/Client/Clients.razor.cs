using CRM.Admin.Components.Dialogs.Client;
using CRM.Admin.Data.ClientDTO;
using CRM.Admin.Requests.ClientRequests;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.Client;

public partial class Clients
{
    [Inject] IDialogService DialogService { get; set; } = default!;
  
    [Inject] IClientRequest ClientRequest { get; set; } = default!;
    private IEnumerable<ClientDTO> pagedData;
    private MudTable<ClientDTO> table;
    private DialogOptions dialogOptions = new() 
    {   
        CloseOnEscapeKey = true, 
        CloseButton = true, 
        DisableBackdropClick = true,
        MaxWidth = MaxWidth.ExtraSmall, 
        FullWidth = true 
    };

    private int totalItems;
    private string searchString = null;
    private Guid? _Id;
   
    private async Task<TableData<ClientDTO>> ServerReload(TableState state)
    {
            IEnumerable<ClientDTO> data = await ClientRequest.GetAllAsync();
            await Task.Delay(300);
            data = data.Where(element =>
            {
                if (string.IsNullOrWhiteSpace(searchString))
                    return true;
                if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (element.Surname.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if ($"{element.Phone}".Contains(searchString))
                    return true;
                if ($"{element.LatestStatus}".Contains(searchString))
                    return true;
                return false;
            }).ToArray();
            totalItems = data.Count();
            switch (state.SortLabel)
            {
                case "name_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Name);
                    break;
                case "surname_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Surname);
                    break;
                case "phone_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Phone);
                    break;
                case "manager_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.ManagerNames.FirstOrDefault());
                    break;
                case "status_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.LatestStatus);
                    break;
            }

            pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
            return new TableData<ClientDTO>() { TotalItems = totalItems, Items = pagedData };
        }

    private void OnSearch(string text)
    {
        searchString = text;
        table.ReloadServerData();
    }
    
    private async Task AddComment(Guid clientId)
    {
        var parameters = new DialogParameters { { "Id", clientId } };
        var dialogReference = await DialogService.ShowAsync<CreateCommentForClientDialog>("", parameters,  dialogOptions);
        var dialogResult = await dialogReference.Result;

        if (dialogResult.Canceled)
            return;

        await table.ReloadServerData();
    }
    
    private async Task AddManager(Guid clientId)
    {
        var parameters = new DialogParameters { { "Id", clientId } };
        var dialogReference = await DialogService.ShowAsync<AddManagerForClientDialog>("", parameters,  dialogOptions);
        var dialogResult = await dialogReference.Result;
        
        if (dialogResult.Canceled)
            return;
        
        await table.ReloadServerData();
    } 
    
    private async Task AddStatus(Guid clientId)
    {
        var parameters = new DialogParameters { { "Id", clientId } };
        var dialogReference = await DialogService.ShowAsync<AddStatusForClientDialog>("", parameters,  dialogOptions);
        var dialogResult = await dialogReference.Result;
        
        if (dialogResult.Canceled)
            return;
        
        await table.ReloadServerData();
    }
}
