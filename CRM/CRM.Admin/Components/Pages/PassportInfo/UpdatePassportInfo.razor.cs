using CRM.Admin.Data.ClientPrivateDataDto;
using CRM.Admin.Data.PassportInfoDto;
using CRM.Admin.Requests.ClientPrivateDataRequests;
using CRM.Admin.Requests.PassportInfoRequests;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CRM.Admin.Components.Pages.PassportInfo;

public partial class UpdatePassportInfo
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IPassportInfoRequest PassportInfoRequest { get; set; } = null!;
    [Inject] public IClientPrivateDataRequest ClientPrivateDataRequest { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    [Parameter] public string ClientId { get; set; } = null!;

    private IEnumerable<PassportInfoDto> _passportInfoDtos = new List<PassportInfoDto>();
    private PassportInfoUpdateDto _passportInternalDto = new();
    private PassportInfoUpdateDto _passportInternationalDto = new();
    private ClientPrivateDataDto _clientPrivateDataDto = new ();
    
    private DateTime? _passportInternalExpiryDate;
    private DateTime? _passportInternalIssueDate;
    private DateTime? _passportInternationalExpiryDate;
    private DateTime? _passportInternationalIssueDate;
    private bool _isSubmissionSuccessful = false;
    private Guid _id;

    protected override async Task OnInitializedAsync()
    {
        _id = Guid.Parse(ClientId);
        _clientPrivateDataDto = await ClientPrivateDataRequest.GetByClientIdAsync<ClientPrivateDataDto>(_id);
    
        _passportInfoDtos = await PassportInfoRequest.GetByClientPrivateDataIdAsync(_clientPrivateDataDto.Id);
        var internalPassport = _passportInfoDtos.FirstOrDefault(s => s.PassportType == PassportType.Internal);
        var internationalPassport = _passportInfoDtos.FirstOrDefault(s => s.PassportType == PassportType.International);
        
        if (internalPassport != null)
        {
            _passportInternalDto = await PassportInfoRequest.GetByIdAsync<PassportInfoUpdateDto>(internalPassport.Id);
            _passportInternalExpiryDate = _passportInternalDto.ExpiryDate.ToDateTime(TimeOnly.MinValue);
            _passportInternalIssueDate = _passportInternalDto.DateOfIssue.ToDateTime(TimeOnly.MinValue);
        }
        else
        {
            Snackbar.Add($"Помилка ", Severity.Error);
        }

        if (internationalPassport != null)
        {
            _passportInternationalDto =
                await PassportInfoRequest.GetByIdAsync<PassportInfoUpdateDto>(internationalPassport.Id);
            _passportInternationalExpiryDate = _passportInternationalDto.ExpiryDate.ToDateTime(TimeOnly.MinValue);
            _passportInternationalIssueDate = _passportInternationalDto.DateOfIssue.ToDateTime(TimeOnly.MinValue);
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
            await PassportInfoRequest.UpdateAsync(_passportInternalDto);
            Snackbar.Add("Внесено зміни до внутрішнього паспорту", Severity.Success);
            _isSubmissionSuccessful = true;
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
            await PassportInfoRequest.UpdateAsync(_passportInternationalDto);
            Snackbar.Add("Внесено зміни до закордонного паспорту", Severity.Success);
            _isSubmissionSuccessful = true;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Помилка: {ex.Message}", Severity.Error);
        }
    }
    
    private void NavigateBack()
    {
        NavigationManager.NavigateTo($"/clientById/{ClientId}"); 
    }
}

