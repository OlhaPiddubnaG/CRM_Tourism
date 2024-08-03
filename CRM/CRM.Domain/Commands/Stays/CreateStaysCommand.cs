using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Stays;

public class CreateStaysCommand : IRequest<CreatedResponse>
{
    public Guid OrderId { get; set; }
    public Guid HotelId { get; set; }
    public DateTime CheckInDate { get; set; } = DateTime.UtcNow;
    public int NumberOfNights { get; set; }
    public string Comment { get; set; } = "";
}
