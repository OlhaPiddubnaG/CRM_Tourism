namespace CRM.Domain.Responses.Authentication;

public record LoginUserResponse(Guid Id, string AccessToken, string RefreshToken);