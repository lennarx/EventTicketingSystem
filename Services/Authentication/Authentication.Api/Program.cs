using Authentication.Api.Data;
using Authentication.Api.Features.Login;
using Authentication.Api.Features.UserRegistration;
using Authentication.Api.Utils.Jwt.Implementations;
using Authentication.Api.Utils.Jwt.Interfaces;
using Shared.Extensions;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplicationServices(typeof(Program).Assembly, addEventPublisher: false);
        builder.Services.AddRequiredDbContext<AuthDbContext>(builder.Configuration);
        builder.Services.AddScoped<IJwtProvider, JwtProvider>();
        builder.Services.AddHostedService<UserRegisteredEventConsumer>();

        var app = builder.Build();

        await app.Services.ApplyMigrationsIfNeeded<AuthDbContext>();
        //Endpoints
        app.MapLogin();

        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.Run();

    }
}

