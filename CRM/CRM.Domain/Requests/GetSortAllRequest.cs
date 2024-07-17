using MediatR;
using MudBlazor;

namespace CRM.Domain.Requests;

public record GetSortAllRequest<T> : IRequest<TableData<T>>
{
    public int page { get; }
    public int pageSize { get; }
    public string searchString { get; }
    public string sortLabel { get; }
    public SortDirection sortDirection { get; }

    public GetSortAllRequest(int page, int pageSize, string searchString, string sortLabel, SortDirection sortDirection)
    {
        page = page;
        pageSize = pageSize;
        searchString = searchString ?? string.Empty;
        sortLabel = !string.IsNullOrWhiteSpace(sortLabel) ? sortLabel : "Name";
        sortDirection = sortDirection;
    }
}