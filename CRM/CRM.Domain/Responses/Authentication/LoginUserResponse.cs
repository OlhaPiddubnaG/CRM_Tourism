namespace CRM.Domain.Responses.Authentication;

public record LoginUserResponse(string Name, string Email, string AccessToken, string RefreshToken);