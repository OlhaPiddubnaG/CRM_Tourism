using MediatR;

namespace CRM.Domain.Requests;

public record GetFilteredAllRequest<T> : IRequest<List<T>>
{
    public string SearchString { get; }

    public GetFilteredAllRequest(string searchString)
    {
        SearchString = searchString ?? string.Empty;
    }
}