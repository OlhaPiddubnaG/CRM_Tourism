namespace CRM.Admin.Data.ClientDto;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalItems { get; set; }
}