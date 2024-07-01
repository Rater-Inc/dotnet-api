using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Rater.Business.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StackExchange.Redis;

namespace Rater.Business.Services
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public JwtTokenService(IHttpContextAccessor httpContextAccessor , IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
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

        public async Task<bool> CreateToken(string token)
        {
            

                var redisConnectionString = _config.GetSection("ConnectionStrings:RedisConnection").Value;
                if (string.IsNullOrEmpty(redisConnectionString))
                    throw new Exception("Redis connection string is missing in the configuration");

            try
            {

                using var redis = ConnectionMultiplexer.Connect(redisConnectionString);
                IDatabase db = redis.GetDatabase();

                string key = token;
                string value = "Valid";
                TimeSpan expiry = TimeSpan.FromHours(24);
                bool setResult = await db.StringSetAsync(key, value, expiry);

                if (!setResult)
                    throw new Exception("Failed to set value in Redis");

                return true;

            }
            catch (RedisConnectionException ex)
            {
                throw new InvalidOperationException("Failed to create token due to Redis connection issue", ex);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> ValidateToken()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (authorizationHeader == null) throw new Exception("Token Could not found");
            var token = authorizationHeader.StartsWith("bearer ") ? authorizationHeader.Substring("bearer ".Length).Trim() : authorizationHeader;

            var redisConnectionString = _config.GetSection("ConnectionStrings:RedisConnection").Value;
            if (string.IsNullOrEmpty(redisConnectionString))
                throw new Exception("Redis connection string is missing in the configuration");

            try
            {
                using var redis = ConnectionMultiplexer.Connect(redisConnectionString);
                IDatabase db = redis.GetDatabase();

                var tokenExist = await db.KeyExistsAsync(token);
                
                if (tokenExist)
                {
                    await db.KeyDeleteAsync(token);
                }

                return tokenExist;

            }
            catch(RedisConnectionException ex)
            {
                throw new InvalidOperationException("Failed to validate token due to Redis connection issue", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
