namespace CRM.Domain.Entities.Base;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    DateTime? DeletedAt { get; set; }
    Guid CreatedUserId { get; set; }
    Guid UpdatedUserId { get; set; }
    Guid DeletedUserId { get; set; }
}