using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class Hotel : Auditable
{
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    public Guid RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }
    public string Name { get; set; } = null!;
    public string Comment { get; set; }
    public List<Meals> Meals { get; set; } = new();
}