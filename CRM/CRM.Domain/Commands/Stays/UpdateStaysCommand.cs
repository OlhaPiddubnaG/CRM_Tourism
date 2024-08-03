using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Stays;

public class UpdateStaysCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid HotelId { get; set; }
    public DateTime CheckInDate { get; set; }
    public int NumberOfNights { get; set; }
    public string Comment { get; set; } = "";
}