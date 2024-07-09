using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Rater.Business.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StackExchange.Redis;
using RedLockNet;

namespace Rater.Business.Services
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly IConnectionMultiplexer _redis;
        private readonly IDistributedLockFactory _distributedLock;
        public JwtTokenService(IHttpContextAccessor httpContextAccessor , 
            IConfiguration config, 
            IConnectionMultiplexer redis,
            IDistributedLockFactory distributedLock)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _redis = redis;
            _distributedLock = distributedLock;
        }

        public string GetHeaderToken()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (authorizationHeader == null) throw new Exception("Token Could not found");
            var token = authorizationHeader.StartsWith("bearer ") ? authorizationHeader.Substring("bearer ".Length).Trim() : authorizationHeader;
            return token;
        }


        public int GetSpaceIdFromToken ()
        {
            var token = GetHeaderToken();
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

                var resource = $"lock:token:{token}";
                var expiry = TimeSpan.FromSeconds(30);
                var wait = TimeSpan.FromSeconds(10);
                var retry = TimeSpan.FromSeconds(1);


                using(var redLock = await _distributedLock.CreateLockAsync(resource,expiry,wait,retry))
                {
                    if(redLock.IsAcquired)
                    {
                        IDatabase db = _redis.GetDatabase();
                        var tokenTTL = Convert.ToInt32(_config.GetSection("jwt:jwtTokenTTL").Value);

                        string key = token;
                        string value = "Valid";
                        TimeSpan tokenExpiry = TimeSpan.FromHours(tokenTTL);
                        bool setResult = await db.StringSetAsync(key, value, tokenExpiry);

                        if (!setResult)
                            throw new Exception("Failed to set value in Redis");

                        return true;
                    }

                    else
                    {
                        throw new Exception("Failed to acquire lock for token creation");
                    }
                }

                

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

            var token = GetHeaderToken();

            var redisConnectionString = _config.GetSection("ConnectionStrings:RedisConnection").Value;
            if (string.IsNullOrEmpty(redisConnectionString))
                throw new Exception("Redis connection string is missing in the configuration");

            try
            {

                var resource = $"lock:token:{token}";
                var expiry = TimeSpan.FromSeconds(30);
                var wait = TimeSpan.FromSeconds(10);
                var retry = TimeSpan.FromSeconds(1);

                using (var redLock = await _distributedLock.CreateLockAsync(resource, expiry, wait, retry))
                {
                    if(redLock.IsAcquired)
                    {
                        IDatabase db = _redis.GetDatabase();

                        var tokenExist = await db.KeyExistsAsync(token);

                        if (tokenExist)
                        {
                            await db.KeyDeleteAsync(token);
                        }

                        return tokenExist;
                    }

                    else
                    {
                        throw new Exception("Failed to acquire lock for token creation");
                    }
                }
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
