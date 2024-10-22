using CRM.Domain.Commands;
using CRM.Domain.Commands.Order;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;

namespace CRM.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ISender _sender;

    public OrderController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdRequest<OrderResponse>(id), token);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var response = await _sender.Send(new GetAllRequest<OrderResponse>(), token);

        return Ok(response);
    }
    
    [HttpPost("paged")]
    [ProducesResponseType(typeof(TableData<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPagedFilteredAndSorted(
        [FromQuery] string? searchString,
        [FromQuery] string? sortLabel,
        [FromQuery] SortDirection sortDirection,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        CancellationToken token)
    {
        var request = new GetFilteredAndSortAllRequest<OrderResponse>(
            searchString,
            sortLabel,
            sortDirection,
            page,
            pageSize);

        var response = await _sender.Send(request, token);

        return Ok(response);
    } 
    
    [HttpPost("pagedByClient")]
    [ProducesResponseType(typeof(TableData<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPagedFilteredAndSortedByClientId(
        [FromQuery] string clientId,
        [FromQuery] string? searchString,
        [FromQuery] string? sortLabel,
        [FromQuery] SortDirection sortDirection,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        CancellationToken token)
    {
        if (!Guid.TryParse(clientId, out Guid parsedClientId))
        {
            return BadRequest("Invalid Client ID format.");
        }
        var request = new GetFilteredAndSortAllWithIdRequest<OrderResponse>(
            parsedClientId,
            searchString,
            sortLabel,
            sortDirection,
            page,
            pageSize);

        var response = await _sender.Send(request, token);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }
    
    [HttpPost("withRelated")]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateOrderWithRelatedCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new DeleteCommand<Order>(id), token);

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateOrderCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }
}