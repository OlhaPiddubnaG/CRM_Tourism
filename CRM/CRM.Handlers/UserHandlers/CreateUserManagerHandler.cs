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

public class CreateUserManagerHandler : IRequestHandler<CreateUserManagerCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateUserManagerHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreatedResponse> Handle(CreateUserManagerCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (existingUser)
        {
            throw new ExistException();
        }

        var hashedPassword = ComputeSha256Hash(request.Password);
        var userManager = _mapper.Map<User>(request);
        userManager.Password = hashedPassword;
        _context.Users.Add(userManager);

        var userRoles = new UserRoles()
        {
            UserId = userManager.Id,
            RoleType = RoleType.Manager,
        };

        _mapper.Map<UserRoles>(userRoles);
        _context.UserRoles.Add(userRoles);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Company), ex);
        }

        return new CreatedResponse(userManager.Id);
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