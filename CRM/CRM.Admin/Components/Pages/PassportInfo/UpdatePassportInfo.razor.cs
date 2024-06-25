using CRM.Admin.Data.ClientPrivateDataDTO;
using CRM.Admin.Data.PassportInfoDTO;
using CRM.Admin.Requests.ClientPrivateDataRequests;
using CRM.Admin.Requests.PassportInfoRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.PassportInfo;

public partial class UpdatePassportInfo
{
    [Parameter] public string clientId { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }
    [Inject] public IPassportInfoRequest PassportInfoRequest { get; set; }
    [Inject] public IClientPrivateDataRequest ClientPrivateDataRequest { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }

    private PassportInfoUpdateDTO passportInternalDTO = new();
    private PassportInfoUpdateDTO passportInternationalDTO = new();
    private IEnumerable<ClientPrivateDataDTO> clientPrivateDataDTOs = new List<ClientPrivateDataDTO>();
    private IEnumerable<PassportInfoDTO> passportInfoDTOs = new List<PassportInfoDTO>();
    
    private DateTime? passportInternalExpiryDate;
    private DateTime? passportInternalIssueDate;
    private DateTime? passportInternationalExpiryDate;
    private DateTime? passportInternationalIssueDate;
    
    private bool isSubmissionSuccessful = false;

    protected override async Task OnInitializedAsync()
    {
        clientPrivateDataDTOs = (await ClientPrivateDataRequest.GetAllAsync())
            .Where(i => i.ClientId == Guid.Parse(clientId));

        var clientPrivateDataIds = clientPrivateDataDTOs.Select(cpd => cpd.Id).ToList();
        passportInfoDTOs = (await PassportInfoRequest.GetAllAsync())
            .Where(pi => clientPrivateDataIds.Contains(pi.ClientPrivateDataId));

        var internalPassport = passportInfoDTOs.FirstOrDefault(s => s.PassportType == PassportType.Internal);
        var internationalPassport = passportInfoDTOs.FirstOrDefault(s => s.PassportType == PassportType.International);
        if (internalPassport != null)
        {
            passportInternalDTO = await PassportInfoRequest.GetByIdAsync<PassportInfoUpdateDTO>(internalPassport.Id);
            passportInternalExpiryDate = passportInternalDTO.ExpiryDate.ToDateTime(TimeOnly.MinValue);
            passportInternalIssueDate = passportInternalDTO.DateOfIssue.ToDateTime(TimeOnly.MinValue);
        }
        else
        {
            Snackbar.Add($"Помилка ", Severity.Error);
        }

        if (internationalPassport != null)
        {
            passportInternationalDTO =
                await PassportInfoRequest.GetByIdAsync<PassportInfoUpdateDTO>(internationalPassport.Id);
            passportInternationalExpiryDate = passportInternationalDTO.ExpiryDate.ToDateTime(TimeOnly.MinValue);
            passportInternationalIssueDate = passportInternationalDTO.DateOfIssue.ToDateTime(TimeOnly.MinValue);
        }
        else
        {
            Snackbar.Add($"Помилка", Severity.Error);
        }
    }

    private async Task SubmitInternal()
    {
        try
        {
            await PassportInfoRequest.UpdateAsync(passportInternalDTO);
            Snackbar.Add("Внесено зміни до внутрішнього паспорту", Severity.Success);
            isSubmissionSuccessful = true;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }

    private async Task SubmitInternational()
    {
        try
        {
            await PassportInfoRequest.UpdateAsync(passportInternationalDTO);
            Snackbar.Add("Внесено зміни до закордонного паспорту", Severity.Success);
            isSubmissionSuccessful = true;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }
    
    private void NavigateBack()
    {
        NavigationManager.NavigateTo($"/clientById/{clientId}"); 
    }
}

