using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.User;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.UserHandlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateUserHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreatedResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        request.Email = request.Email.ToUpper();

        var existingUser = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (existingUser)
        {
            throw new ExistException();
        }

        var hashedPassword = ComputeSha256Hash(request.Password);
        var user = _mapper.Map<User>(request);
        user.Password = hashedPassword;
        user.CreatedAt = DateTime.UtcNow;
        _context.Users.Add(user);

        foreach (var roleType in request.RoleTypes)
        {
            if (roleType == RoleType.Admin)
            {
                throw new InvalidOperationException("Creating a user with the Admin role is not allowed.");
            }

            var userRole = new UserRoles()
            {
                UserId = user.Id,
                RoleType = roleType,
            };

            user.UserRoles.Add(userRole);
            _context.UserRoles.Add(userRole);
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Company), ex);
        }

        return new CreatedResponse(user.Id);
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