namespace CRM.Domain.Entities.Base;

public abstract class Auditable : BaseEntity, IAuditable
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid CreatedUserId { get; set; }
    public Guid UpdatedUserId { get; set; }
    public Guid DeletedUserId { get; set; }
}
