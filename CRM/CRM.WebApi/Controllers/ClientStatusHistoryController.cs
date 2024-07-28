using CRM.Domain.Requests;
using CRM.Domain.Responses.ClientStatusHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientStatusHistoryController : ControllerBase
{
    private readonly ISender _sender;

    public ClientStatusHistoryController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("client/{clientId:guid}")]
    [ProducesResponseType(typeof(List<ClientStatusHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByClientId([FromRoute] Guid clientId, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdReturnListRequest<ClientStatusHistoryResponse>(clientId), token);

        return Ok(response);
    }
}