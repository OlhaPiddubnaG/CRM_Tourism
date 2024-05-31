using System.Security.Claims;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace CRM.Handlers.Services.CurrentUser;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public RoleType GetRoles()
    {
        var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
        if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var roleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role);
        if (roleClaim == null || !Enum.TryParse<RoleType>(roleClaim.Value, out var role))
        {
            throw new UnauthorizedAccessException("Role claim not found or invalid.");
        }

        return role;
    }

    public Guid GetCompanyId()
    {
        var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
        if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var companyId = Guid.Parse(claimsPrincipal.FindFirst(ClaimTypes.GroupSid).Value);
        if (companyId == null)
        {
            throw new UnauthorizedAccessException("Company not found in the database.");
        }

        return companyId;
    }

    public Guid GetUserId()
    {
        var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
        if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID not found in claims.");
        }

        return Guid.Parse(userIdClaim.Value);
    }
}