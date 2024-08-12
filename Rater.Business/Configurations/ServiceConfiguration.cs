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
using Rater.Data.Repositories.Decorator;
using Microsoft.AspNetCore.Authorization;
using Rater.API;
using Rater.Business.Validator.Dto.Rating;
using Rater.Business.Validator.Dto.Space;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace Rater.Business.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DBBContext>();
            services.AddMemoryCache();

            services.AddScoped<SpaceRepository>();
            services.AddScoped<ISpaceRepository, CachedSpaceRepository>();
            services.AddScoped<ISpaceService, SpaceService>();

            services.AddScoped<RatingRepository>();
            services.AddScoped<IRatingRepository, CachedRatingRepository>();
            services.AddScoped<IRatingService, RatingService>();

            services.AddScoped<MetricRepository>();
            services.AddScoped<IMetricRepository, CachedMetricRepository>();
            services.AddScoped<IMetricService, MetricService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ParticipantRepository>();
            services.AddScoped<IParticipantRepository, CachedParticipantRepository>();
            services.AddScoped<IParticipantService, ParticipantService>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();

            services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<RatingRequestDtoValidator>()
                .AddValidatorsFromAssemblyContaining<GrandSpaceRequestDtoValidator>();

            return services;
        }

        public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
        {

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
