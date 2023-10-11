using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserTestCRUD.BusinessLogic.Interfaces;
using UserTestCRUD.DAL.DataBaseContext;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.BusinessLogic.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _symmetricSecurityKey;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public TokenService(IConfiguration configuration, ApplicationDbContext context)
        {
            _context = context;
            _configuration = configuration;
            var authenticationSection = _configuration.GetSection("Authentication");
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSection["Secret"]));
        }

        public JwtSecurityToken GenerateJwtToken(User user)
        {
            var userWithRole = _context.Users.Include(u => u.Roles).FirstOrDefault(x=> x.Name == user.Name);

            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var userRoles = userWithRole.Roles.Select(r => r.Name);

            foreach(var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                 issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:TokenLifetimeMinutes"])),
                signingCredentials: creds,
                notBefore: DateTime.UtcNow);

            return tokenDescriptor;
        }

        public string GetToken(User user)
        {
            var jwtToken = GenerateJwtToken(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedJwt = tokenHandler.WriteToken(jwtToken);

            return encodedJwt;
        }

        public int GetUserIdFromClaims(ClaimsPrincipal claimsPrincipal)
        {
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdClaim.Value);

            return userId;
        }
    }
}
