using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CRM.Core.Configuration;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Authentication;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Authentication;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CRM.Handlers.AuthenticationHandlers
{
    public class LoginHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
    {
        private readonly AppDbContext _context;
        private readonly IOptions<JwtConfiguration> _jwtConfiguration;

        public LoginHandler(AppDbContext context, IOptions<JwtConfiguration> jwtConfiguration)
        {
            _context = context;
            _jwtConfiguration = jwtConfiguration;
        }

        public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null || !VerifyPassword(user.Password, request.Password))
            {
                throw new AuthorizationFailedException();
            }

            return await CreateToken(populateExp: true, user);
        }

        private async Task<LoginUserResponse> CreateToken(bool populateExp, User user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;

            if (populateExp)
            {
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new LoginUserResponse(user.Id, accessToken, refreshToken);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Value.TokenKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.GroupSid, user.CompanyId.ToString()),
                new Claim(ClaimTypes.Expiration,
                    DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtConfiguration.Value.Expires)).ToString("O")),
            };

            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.RoleType.ToString()));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            return new JwtSecurityToken(
                issuer: _jwtConfiguration.Value.ValidIssuer,
                audience: _jwtConfiguration.Value.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtConfiguration.Value.Expires)),
                signingCredentials: signingCredentials);
        }

        private bool VerifyPassword(string storedPassword, string enteredPassword)
        {
            var hashedEnteredPassword = ComputeSha256Hash(enteredPassword);

            return storedPassword.Equals(hashedEnteredPassword);
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