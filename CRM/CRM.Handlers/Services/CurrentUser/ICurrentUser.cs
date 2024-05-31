using CRM.Domain.Enums;

namespace CRM.Handlers.Services.CurrentUser;

public interface ICurrentUser
{
    RoleType GetRoles();
    Guid GetCompanyId();
    Guid GetUserId();
}