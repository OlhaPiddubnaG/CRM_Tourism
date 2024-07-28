using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Client;

public class UpdateClientCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid CountryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Surname { get; set; }
    public string? Patronymic { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Comment { get; set; }
    public SourceOfEngagement SourceOfEngagement { get; set; }
    public List<Guid> ManagerIds { get; set; } = new();
    public ClientStatus LatestStatus { get; set; }
}