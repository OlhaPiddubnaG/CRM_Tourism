using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Authentication;
using CRM.Domain.Entities;
using CRM.Handlers.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CRM.Handlers.AuthenticationHandlers;

public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly IEmail _email;
    private readonly IConfiguration _configuration;

    public ForgotPasswordHandler(AppDbContext context, IEmail email, IConfiguration configuration)
    {
        _context = context;
        _email = email;
        _configuration = configuration;
    }

    public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var domain = _configuration.GetSection("AppSettings");
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(typeof(User));
        }

        user.PasswordResetToken = Guid.NewGuid().ToString();
        user.PasswordResetTokenExpiryTime = DateTime.UtcNow.AddHours(1);

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        var resetLink = $"{domain["Domain"]}/reset-password?token={user.PasswordResetToken}";
        var emailBody = $@"<p>Reset your password using this link: <a href='{resetLink}'>click here</a></p>";

        await _email.SendEmailAsync(user.Email, "Password Reset", emailBody);

        return Unit.Value;
    }
}