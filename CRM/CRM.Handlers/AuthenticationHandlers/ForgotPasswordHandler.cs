using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Authentication;
using CRM.Domain.Entities;
using CRM.Handlers.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.AuthenticationHandlers;

public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly IEmail _email;

    public ForgotPasswordHandler(AppDbContext context, IEmail email)
    {
        _context = context;
        _email = email;
    }

    public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(typeof(User));
        }

        user.PasswordResetToken = Guid.NewGuid().ToString();
        user.PasswordResetTokenExpiryTime = DateTime.UtcNow.AddHours(1);

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        var resetLink = $"https://yourapp.com/reset-password?token={user.PasswordResetToken}";
        await _email.SendEmailAsync(user.Email, "Password Reset", $"Reset your password using this link: {resetLink}");

        return Unit.Value;
    }
}