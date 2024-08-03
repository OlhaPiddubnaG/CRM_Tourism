using CRM.Domain.Commands;
using CRM.Domain.Commands.Touroperator;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.Touroperator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TouroperatorController : ControllerBase
{
    private readonly ISender _sender;

    public TouroperatorController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TouroperatorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdRequest<TouroperatorResponse>(id), token);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TouroperatorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var response = await _sender.Send(new GetAllRequest<TouroperatorResponse>(), token);

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateTouroperatorCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }
    
    [HttpPost("filter")]
    [ProducesResponseType(typeof(List<TouroperatorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetFiltredDataAsync([FromQuery] string? searchString,
        CancellationToken token)
    {
        var response = await _sender.Send(new GetFilteredAllRequest<TouroperatorResponse>(searchString), token);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new DeleteCommand<Touroperator>(id), token);

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateTouroperatorCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }
}