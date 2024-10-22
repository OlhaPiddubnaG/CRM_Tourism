using MediatR;
using MudBlazor;

namespace CRM.Domain.Requests;

public record GetFilteredAndSortAllWithIdRequest<T> : IRequest<TableData<T>>
{
    public Guid Id { get; }
    public string SearchString { get; }
    public string SortLabel { get; }
    public SortDirection SortDirection { get; }
    public int Page { get; }
    public int PageSize { get; }

    public GetFilteredAndSortAllWithIdRequest(Guid id, string searchString, string sortLabel, SortDirection sortDirection,
        int page,
        int pageSize)
    {
        Id = id;
        SearchString = searchString ?? string.Empty;
        SortLabel = sortLabel ?? "Name";
        SortDirection = sortDirection;
        Page = page;
        PageSize = pageSize;
    }
}