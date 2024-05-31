using System.ComponentModel.DataAnnotations;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Authentication;
using CRM.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.AuthenticationHandlers;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
    private readonly AppDbContext _context;

    public ResetPasswordHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token, cancellationToken);
        if (user == null || user.PasswordResetTokenExpiryTime < DateTime.UtcNow)
        {
            throw new InvalidTokenException("Invalid or expired token");
        }
        
        if (request.NewPassword != request.ConfirmPassword)
        {
            throw new ValidationException("Passwords do not match");
        }
        
        user.Password = HashHelper.HashWithSha256(request.NewPassword); 
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiryTime = null;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
