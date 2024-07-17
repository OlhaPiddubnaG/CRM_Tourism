namespace CRM.Domain.Responses.Сountry;

public record CountryResponse
{
    public Guid Id { get; set; } 
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } = null!;
    public bool IsDeleted { get; set; } 
};