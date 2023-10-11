using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.BusinessLogic.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateJwtToken(User user);
        string GetToken(User user);
        int GetUserIdFromClaims(ClaimsPrincipal claimsPrincipal);
    }
}
