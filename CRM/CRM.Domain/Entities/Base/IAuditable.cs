namespace CRM.Domain.Entities.Base;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    DateTime? DeletedAt { get; set; }
    Guid UserId { get; set; }
}
