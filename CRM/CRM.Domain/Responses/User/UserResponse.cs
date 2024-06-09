namespace CRM.Domain.Responses.User;

public record UserResponse
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; } 
    public string Surname { get; set; } 
    public string Email { get; set; } 
    public bool IsDeleted { get; set; } 
}