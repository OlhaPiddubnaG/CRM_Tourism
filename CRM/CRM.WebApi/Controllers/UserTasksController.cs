using CRM.Domain.Commands;
using CRM.Domain.Commands.UserTasks;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.UserTasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MudBlazor;

namespace CRM.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserTasksController : ControllerBase
{
    private readonly ISender _sender;

    public UserTasksController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserTasksResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdRequest<UserTasksResponse>(id), token);

        return Ok(response);
    }

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<UserTasksResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByClientId([FromRoute] Guid userId, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdReturnListRequest<UserTasksResponse>(userId), token);

        return Ok(response);
    }
    
    [HttpPost("byDay")]
    [ProducesResponseType(typeof(List<UserTasksResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPagedFiltered(
        [FromQuery] string userId,
        [FromQuery] string dateTime,
        CancellationToken token)
    {
        if (!Guid.TryParse(userId, out Guid parsedUserId))
        {
            return BadRequest("Invalid User ID format.");
        }
        
        if (!DateTime.TryParse(dateTime, out DateTime parsedDateTime))
        {
            return BadRequest("Invalid Date format.");
        }
        if (parsedDateTime.Kind == DateTimeKind.Unspecified)
        {
            parsedDateTime = DateTime.SpecifyKind(parsedDateTime, DateTimeKind.Utc);
        }
        else
        {
            parsedDateTime = parsedDateTime.ToUniversalTime();
        }
        var request = new GetByIdAndDateRequest<UserTasksResponse>(
            parsedUserId,
            parsedDateTime);

        var response = await _sender.Send(request, token);

        return Ok(response);
    }
    
    [HttpPost("paged")]
    [ProducesResponseType(typeof(TableData<UserTasksResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPagedFilteredAndSorted(
        [FromQuery] string userId,
        [FromQuery] string? searchString,
        [FromQuery] string? sortLabel,
        [FromQuery] SortDirection sortDirection,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        CancellationToken token)
    {
        if (!Guid.TryParse(userId, out Guid parsedUserId))
        {
            return BadRequest("Invalid User ID format.");
        }
        var request = new GetFilteredAndSortAllWithIdRequest<UserTasksResponse>(
            parsedUserId,
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
    public async Task<IActionResult> Create([FromBody] CreateUserTasksCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new DeleteCommand<UserTasks>(id), token);

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateUserTasksCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }
}