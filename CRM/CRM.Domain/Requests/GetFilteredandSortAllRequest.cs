using MediatR;
using MudBlazor;

namespace CRM.Domain.Requests;

public record GetFilteredAndSortAllRequest<T> : IRequest<TableData<T>>
{
    public string SearchString { get; }
    public string SortLabel { get; }
    public SortDirection SortDirection { get; }
    public int Page { get; }
    public int PageSize { get; }

    public GetFilteredAndSortAllRequest(string searchString, string sortLabel, SortDirection sortDirection, int page,
        int pageSize)
    {
        SearchString = searchString ?? string.Empty;
        SortLabel = sortLabel ?? "Name";
        SortDirection = sortDirection;
        Page = page;
        PageSize = pageSize;
    }
}