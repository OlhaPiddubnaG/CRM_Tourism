namespace CRM.Domain.Entities.Base;

public abstract class BaseEntity : IHaveId
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; } 
}
