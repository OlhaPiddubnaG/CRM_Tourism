namespace CRM.Admin.Auth;

public class AuthState
{
    public Guid UserId { get; private set; }
    public Guid CompanyId { get; private set; }

    public void SetUserId(Guid userId)
    {
        UserId = userId;
    }

    public void SetCompanyId(Guid companyId)
    {
        CompanyId = companyId;
    }
}