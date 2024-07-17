using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.PassportInfo;

public class CreatePassportInfoCommand : IRequest<CreatedResponse>
{
    public Guid ClientPrivateDataId { get; set; } 
    public string RecordNo { get; set; } = "";
    public string DocumentNo { get; set; } = "";
    public DateOnly DateOfIssue { get; set; } 
    public string  Authority { get; set; } 
    public DateOnly ExpiryDate { get; set; } 
    public string Nationality { get; set; } 
    public string PlaceOfBirth { get; set; } = "";
    public string  TaxpayerNo { get; set; } = "";
    public PassportType PassportType { get; set; }
    public string NameLatinScript { get; set; } = "";
    public string  SurnameLatinScript { get; set; } = "";
}