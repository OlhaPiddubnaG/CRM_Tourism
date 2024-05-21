namespace CRM.DataAccess.InitialAdminSettings;

public class InitialAdminSettings
{
    public const string Section = "AdminPersonalData";

    public required string Email { get; set; }

    public required string Password { get; init; }

    public required string Name { get; init; }

    public required string Surname { get; init; }
    
    public required string CompanyName { get; init; }
}