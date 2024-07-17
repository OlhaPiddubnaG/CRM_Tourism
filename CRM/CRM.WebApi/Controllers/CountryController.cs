using CRM.Domain.Commands;
using CRM.Domain.Commands.Country;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses;
using CRM.Domain.Responses.Сountry;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly ISender _sender;

    public CountryController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CountryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken token)
    {
        var response = await _sender.Send(new GetByIdRequest<CountryResponse>(id), token);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CountryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var response = await _sender.Send(new GetAllRequest<CountryResponse>(), token);

        return Ok(response);
    }
    
    [HttpGet("name/{name}")]
    [ProducesResponseType(typeof(CountryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByName(string name, CancellationToken token)
    {
        var response = await _sender.Send(new GetByNameRequest<CountryResponse>(name), token);

        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadResponseResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateCountryCommand request, CancellationToken token)
    {
        var response = await _sender.Send(request, token);

        return Ok(response);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken token)
    {
        await _sender.Send(new DeleteCommand<Country>(id), token);

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(UpdateCountryCommand request, CancellationToken token)
    {
        await _sender.Send(request, token);

        return NoContent();
    }
}