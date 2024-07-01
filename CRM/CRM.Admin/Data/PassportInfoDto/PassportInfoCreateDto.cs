using CRM.Domain.Enums;

namespace CRM.Admin.Data.PassportInfoDto;

public class PassportInfoCreateDto
{
    public Guid ClientPrivateDataId { get; set; }
    public string RecordNo { get; set; } = string.Empty;
    public string DocumentNo { get; set; } = string.Empty;
    public DateOnly? DateOfIssue { get; set; } 
    public string Authority { get; set; } = string.Empty;
    public DateOnly? ExpiryDate { get; set; } 
    public string Nationality { get; set; } = string.Empty;
    public string PlaceOfBirth { get; set; } = string.Empty;
    public string TaxpayerNo { get; set; } = string.Empty;
    public PassportType PassportType { get; set; }  
    public string NameLatinScript { get; set; } = string.Empty;
    public string SurnameLatinScript { get; set; } = string.Empty;
}