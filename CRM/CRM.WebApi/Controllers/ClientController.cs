using CRM.Domain.Commands;
using CRM.Domain.Commands.Client;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;

namespace CRM.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ISender _sender;

    public ClientController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateClientCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }

    [HttpPost("withRelated")]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateClientWithRelatedCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdRequest<ClientResponse>(id), token);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var response = await _sender.Send(new GetAllRequest<ClientResponse>(), token);

        return Ok(response);
    }

    [HttpPost("paged")]
    [ProducesResponseType(typeof(TableData<ClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? searchString,
        [FromQuery] string? sortLabel,
        [FromQuery] SortDirection sortDirection,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(searchString))
        {
            var allClients = await _sender.Send(new GetAllRequest<ClientResponse>(), token);
            var sortedClients = SortClients(allClients, sortLabel, sortDirection);
            var pagedClients = sortedClients.Skip(page * pageSize).Take(pageSize).ToList();

            return Ok(new TableData<ClientResponse>
            {
                TotalItems = allClients.Count(),
                Items = pagedClients
            });
        }
        else
        {
            var request = new GetFilteredAndSortAllRequest<ClientResponse>(
                searchString,
                sortLabel,
                sortDirection,
                page,
                pageSize);

            var response = await _sender.Send(request, token);

            return Ok(response);
        }
    }

    private IEnumerable<ClientResponse> SortClients(IEnumerable<ClientResponse> clients, string sortLabel,
        SortDirection sortDirection)
    {
        return sortLabel switch
        {
            "Name" => sortDirection == SortDirection.Ascending
                ? clients.OrderBy(c => c.Name)
                : clients.OrderByDescending(c => c.Name),
            "Surname" => sortDirection == SortDirection.Ascending
                ? clients.OrderBy(c => c.Surname)
                : clients.OrderByDescending(c => c.Surname),
            _ => clients
        };
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken token)
    {
        await _sender.Send(new DeleteCommand<Client>(id), token);

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(UpdateClientCommand request, CancellationToken token)
    {
        await _sender.Send(request, token);

        return NoContent();
    }
}