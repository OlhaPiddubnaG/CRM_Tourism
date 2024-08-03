using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class RoomType : BaseEntity
{
    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}