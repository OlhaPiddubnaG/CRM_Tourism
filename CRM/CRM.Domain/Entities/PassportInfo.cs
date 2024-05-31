using CRM.Domain.Entities.Base;
using CRM.Domain.Enums;

namespace CRM.Domain.Entities;

public class PassportInfo : Auditable
{
    public Guid ClientPrivateDataId { get; set; } 
    public ClientPrivateData? ClientPrivateData { get; set; } 
    public string RecordNo { get; set; } 
    public string DocumentNo { get; set; } 
    public DateOnly DateOfIssue { get; set; } 
    public string  Authority { get; set; } 
    public DateOnly ExpiryDate { get; set; } 
    public string Nationality { get; set; } 
    public string PlaceOfBirth { get; set; } 
    public string  TaxpayerNo { get; set; } 
    public PassportType PassportType { get; set; } 
    public string NameLatinScript { get; set; } 
    public string  SurnameLatinScript { get; set; } 
}