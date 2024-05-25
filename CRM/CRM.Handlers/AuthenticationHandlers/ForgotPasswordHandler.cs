using System.Security.Cryptography;
using System.Text;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Authentication;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.AuthenticationHandlers;

public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Unit>
{
    private readonly AppDbContext _context;

    public ForgotPasswordHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (existingUser == null)
        {
            throw new NotFoundException(typeof(User));
        }

        existingUser.Password = ComputeSha256Hash(request.NewPassword);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(User), ex);
        }

        return Unit.Value;
    }

    private static string ComputeSha256Hash(string rawData)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}