using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.RoomType;

public class UpdateRoomTypeCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}