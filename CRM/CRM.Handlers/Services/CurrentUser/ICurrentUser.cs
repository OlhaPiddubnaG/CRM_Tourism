using CRM.Domain.Enums;

namespace CRM.Handlers.Services.CurrentUser;

public interface ICurrentUser
{
    List<RoleType> GetRoles();
    Guid GetCompanyId();
    Guid GetUserId();
}