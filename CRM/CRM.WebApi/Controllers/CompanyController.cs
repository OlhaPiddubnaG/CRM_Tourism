using CRM.Domain.Commands;
using CRM.Domain.Commands.Company;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.Company;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ISender _sender;

    public CompanyController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdRequest<CompanyResponse>(id), token);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CompanyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var response = await _sender.Send(new GetAllRequest<CompanyResponse>(), token);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new DeleteCommand<Company>(id), token);

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ResultBaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(UpdateCompanyCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }
}