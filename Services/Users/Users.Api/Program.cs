using Shared.Extensions;
using Users.Api.Data;
using Users.Api.Features.Register;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplicationServices(typeof(Program).Assembly);
        builder.Services.AddRequiredDbContext<UsersDbContext>(builder.Configuration);
        builder.Services.AddAuthenticationServices(builder.Configuration);
        builder.Services.AddControllers();

        var app = builder.Build();

        await app.Services.ApplyMigrationsIfNeeded<UsersDbContext>();
        //Endpoints
        app.MapRegisterUser();

        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthentication();
        app.UseAuthorization();

        app.Run();

    }
}

