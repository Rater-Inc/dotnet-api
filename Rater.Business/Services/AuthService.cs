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
        private readonly IConfiguration _configuration;
        public AuthService(ISpaceRepository spaceRepository,IConfiguration configuration)
        {
            _spaceRepository = spaceRepository;
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
                    response.jwtToken = CreateToken();
                    response.space_id = space.SpaceId;
                    return response;
                }
                return response;

            }
            catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }


        public string CreateToken()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "DEFAULT"),
                new Claim(ClaimTypes.Role, "user")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("jwt:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }


    }
}
