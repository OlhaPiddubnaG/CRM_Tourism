using CRM.Domain.Enums;

namespace CRM.Handlers.Services;

public interface ICurrentUser
{
    RoleType GetRoleForCurrentUser();
    Guid GetCompanyIdForCurrentUser();
}