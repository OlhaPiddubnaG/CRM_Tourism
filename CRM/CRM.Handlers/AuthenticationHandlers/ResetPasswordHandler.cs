using System.ComponentModel.DataAnnotations;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Authentication;
using CRM.Domain.Responses;
using CRM.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.AuthenticationHandlers;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, ResultBaseResponse>
    {
    private readonly AppDbContext _context;

    public ResetPasswordHandler(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ResultBaseResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token, cancellationToken);
        if (user == null || user.PasswordResetTokenExpiryTime < DateTime.UtcNow)
        {
            throw new InvalidTokenException("Invalid or expired token");
        }
       
        user.Password = HashHelper.HashWithSha256(request.NewPassword); 
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiryTime = null;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return new ResultBaseResponse { Success = true, Message = "Password reset successfully." };
    }
}
