using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.User;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Responses;

namespace CRM.Handlers.UserHandlers
{
    public class CreateUserCompanyAdminHandler : IRequestHandler<CreateUserCompanyAdminCommand, CreatedResponse>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateUserCompanyAdminHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CreatedResponse> Handle(CreateUserCompanyAdminCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (existingUser)
            {
                throw new ExistException();
            }

            var hashedPassword = ComputeSha256Hash(request.Password);

            var userAdmin = _mapper.Map<User>(request);
            userAdmin.Password = hashedPassword;

            _context.Users.Add(userAdmin);

            var userRoles = new UserRoles
            {
                UserId = userAdmin.Id,
                RoleType = RoleType.Boss,
            };

            _context.UserRoles.Add(userRoles);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw new SaveDatabaseException(typeof(User), ex);
            }

            return new CreatedResponse(userAdmin.Id);
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
}