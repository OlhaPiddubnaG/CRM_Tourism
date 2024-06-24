using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.User;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Responses;
using CRM.Helper;
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

        var hashedPassword = HashHelper.HashWithSha256(request.Password);
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
            throw new SaveDatabaseException(typeof(User), ex);
        }

        return new CreatedResponse(user.Id);
    }
}