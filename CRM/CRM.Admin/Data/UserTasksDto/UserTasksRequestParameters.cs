namespace CRM.Admin.Data.UserTasksDto;

public class UserTasksRequestParameters
{
    public Guid UserId { get; set; }
    public DateTime? DateTime { get; set; }
}