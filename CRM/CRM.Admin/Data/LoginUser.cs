namespace CRM.Admin.Data;

public class LoginUser
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}