namespace CRM.Domain.Entities.Base;

public interface IHaveId
{
    Guid Id { get; set; }
    bool IsDeleted { get; set; } 
}
