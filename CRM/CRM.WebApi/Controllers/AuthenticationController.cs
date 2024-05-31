using CRM.Domain.Commands.Authentication;
using CRM.Domain.Responses.Authentication;
using CRM.Handlers.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRM.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IEmail _email;
    private readonly IConfiguration _configuration;

    public AuthenticationController(ISender sender, IEmail email, IConfiguration configuration)
    {
        _sender = sender;
        _email = email;
        _configuration = configuration;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUser(LoginUserCommand user, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(user, cancellationToken);

        return Ok(response);
    }

    [HttpPost("forgotPassword")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        
        return NoContent();
    }

    [HttpPost("resetPassword")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);
        
        return NoContent();
    }
}