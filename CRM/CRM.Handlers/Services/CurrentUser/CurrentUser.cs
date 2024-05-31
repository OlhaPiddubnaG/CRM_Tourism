using System.Security.Claims;
using CRM.Domain.Enums;
using CRM.Helper;
using Microsoft.AspNetCore.Http;

namespace CRM.Handlers.Services.CurrentUser;

public class CurrentUser : ICurrentUser
{
    private readonly List<RoleType> _roles;
    private readonly Guid _companyId;
    private readonly Guid _userId;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        var claimsPrincipal = httpContextAccessor.HttpContext?.User;
        if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        _roles = claimsPrincipal.FindAll(ClaimTypes.Role)
            .Select(c =>
                Enum.TryParse<RoleType>(c.Value, out var role)
                    ? role
                    : throw new UnauthorizedAccessException("Role claim not found or invalid."))
            .ToList();

        var companyIdClaim = claimsPrincipal.FindFirst(CustomClaimTypes.CompanyId);
        if (companyIdClaim == null || !Guid.TryParse(companyIdClaim.Value, out _companyId))
        {
            throw new UnauthorizedAccessException("Company not found in the claims.");
        }

        var userIdClaim = claimsPrincipal.FindFirst(CustomClaimTypes.UserId);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out _userId))
        {
            throw new UnauthorizedAccessException("User ID not found in claims.");
        }
    }

    public List<RoleType> GetRoles() => _roles;
    public Guid GetCompanyId() => _companyId;
    public Guid GetUserId() => _userId;
}