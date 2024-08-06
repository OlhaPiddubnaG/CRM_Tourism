using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Hotel;

public class UpdateHotelCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid RoomTypeId { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
}