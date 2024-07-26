using CRM.Domain.Requests;
using CRM.Domain.Responses.OrderStatusHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrderStatusHistoryController : ControllerBase
{
    private readonly ISender _sender;

    public OrderStatusHistoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderStatusHistoryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdRequest<OrderStatusHistoryResponse>(id), token);

        return Ok(response);
    }

    [HttpGet("order/{orderId:guid}")]
    [ProducesResponseType(typeof(List<OrderStatusHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll([FromRoute] Guid orderId, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdReturnListRequest<OrderStatusHistoryResponse>(orderId), token);

        return Ok(response);
    }
}