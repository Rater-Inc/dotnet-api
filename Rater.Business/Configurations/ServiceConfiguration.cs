using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rater.Business.Services.Interfaces;
using Rater.Business.Services;
using Rater.Data.DataContext;
using Rater.Data.Repositories.Interfaces;
using Rater.Data.Repositories;
using RedLockNet.SERedis.Configuration;
using RedLockNet.SERedis;
using RedLockNet;
using StackExchange.Redis;

namespace Rater.Business.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DBBContext>();


            services.AddScoped<ISpaceRepository, SpaceRepository>();
            services.AddScoped<ISpaceService, SpaceService>();

            services.AddScoped<IMetricRepository, MetricRepository>();
            services.AddScoped<IMetricService, MetricService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IParticipantService, ParticipantService>();

            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IRatingService, RatingService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<IJwtTokenService, JwtTokenService>();

            return services;
        }

        public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration) {

            var redisConnectionString = configuration.GetSection("ConnectionStrings:RedisConnection").Value!;
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            services.AddSingleton<IConnectionMultiplexer>(redis);

            var multiplexers = new List<RedLockMultiplexer> { new RedLockMultiplexer(redis) };
            var redlockfactory = RedLockFactory.Create(multiplexers);
            services.AddSingleton<IDistributedLockFactory>(redlockfactory);

            return services;
        }
    }
}
