using CRM.Domain.Commands;
using CRM.Domain.Commands.RoomType;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.RoomType;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoomTypeController : ControllerBase
{
    private readonly ISender _sender;

    public RoomTypeController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<RoomTypeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var response = await _sender.Send(new GetAllRequest<RoomTypeResponse>(), token);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateRoomTypeCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new DeleteCommand<RoomType>(id), token);

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateRoomTypeCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }
}