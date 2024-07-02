using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rater.Business.Services.Interfaces;
using Rater.Data.Repositories.Interfaces;
using Rater.Domain.DataTransferObjects.AuthDto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rater.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly ISpaceRepository _spaceRepository;
        private readonly IJwtTokenService _tokenService;
        private readonly IConfiguration _configuration;
        
        public AuthService(ISpaceRepository spaceRepository,IJwtTokenService tokenService,IConfiguration configuration)
        {
            _spaceRepository = spaceRepository;
            _tokenService = tokenService;
            _configuration = configuration;
        }


        public async Task<AuthResponseDto> AuthLobby (string link , string password)
        {

            try
            {
                AuthResponseDto response = new AuthResponseDto();
                var space = await _spaceRepository.GetSpaceByLink(link);
                if(BCrypt.Net.BCrypt.Verify(password,space.Password))
                {
                    response.Success = true;
                    response.spaceId = space.SpaceId;
                    response.jwtToken = CreateToken(space.SpaceId);
                    await _tokenService.CreateToken(response.jwtToken);
                    return response;
                }

                return response;

            }
            catch(InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }


        public string CreateToken(int space_id)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier , space_id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("jwt:Token").Value!));
            var tokenTTL = Convert.ToInt32(_configuration.GetSection("jwt:jwtTokenTTL").Value);

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(tokenTTL),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }


        public async Task ValidateAuthorization(int spaceId)
        {
            if (_tokenService.GetSpaceIdFromToken() != spaceId || !await _tokenService.ValidateToken())
            {
                throw new UnauthorizedAccessException("Unauthorized for this space");
            }

        }


    }
}
