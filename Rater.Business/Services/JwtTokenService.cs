using Microsoft.AspNetCore.Http;
using Rater.Business.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Rater.Business.Services
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public JwtTokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetSpaceIdFromToken ()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (authorizationHeader == null) throw new Exception("Token Could not found");
            var token = authorizationHeader.StartsWith("bearer ") ? authorizationHeader.Substring("bearer ".Length).Trim() : authorizationHeader;
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToList();
            var nameIdentifierClaim = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier);
            int spaceId = int.Parse(nameIdentifierClaim.Value);

            return spaceId;
        }

    }
}
