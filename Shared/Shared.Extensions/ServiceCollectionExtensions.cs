
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using Shared.Messaging.Implementations;
using Shared.Messaging.Interfaces;
using System.Reflection;
using System.Text;

namespace Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, Assembly assembly, bool addEventPublisher = true, bool addMediaTr = true)
        {
            if (addMediaTr)
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            if (addEventPublisher)
                services.AddMessagingQueue();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        public static async Task ApplyMigrationsIfNeeded<TContext>(this IServiceProvider serviceProvider) where TContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

            if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
                dbContext.Database.Migrate();
        }

        public static void AddRequiredDbContext<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(connectionString));
        }

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                        )
                    };
                });
            services.AddAuthorization();
            return services;
        }

        private static IServiceCollection AddMessagingQueue(this IServiceCollection services)
        {
            services.AddSingleton<IEventPublisher>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var factory = new ConnectionFactory
                {
                    HostName = config["Rabbit:HostName"] ?? "localhost"
                };

                var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                var channel = connection.CreateChannelAsync().GetAwaiter().GetResult();

                return new EventPublisher(channel);
            });
            return services;
        }
    }
}
