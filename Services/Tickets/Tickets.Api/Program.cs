using MassTransit;
using Shared.Extensions;
using System.Net.Sockets;
using Tickets.Api.Consumers;
using Tickets.Api.GraphQL.Mutations;
using Tickets.Api.GraphQL.Queries;
using Tickets.Api.GraphQL.Types;
using Tickets.Application.Features.UserRegistration;
using Tickets.Application.Services;
using Tickets.Infrastructure;
using Tickets.Infrastructure.Repositories;
using Tickets.Infrastructure.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddApplicationServices(typeof(Program).Assembly, addMediaTr: false);
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<CancelReservationCommandConsumer>();
            x.AddConsumer<ConfirmReservationCommandConsumer>();
            x.AddConsumer<CreateReservationCommandConsumer>();
            var rabbitHost = builder.Configuration["Rabbit:HostName"];

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitHost, "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
        builder.Services.AddRequiredDbContext<TicketsDbContext>(builder.Configuration);
        builder.Services.AddHostedService<UserRegisteredEventConsumer>();
        builder.Services.AddAuthenticationServices(builder.Configuration);
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IReservationService, ReservationService>();

        builder.Services
            .AddGraphQLServer()
            .AddAuthorizationCore()
            .AddQueryType<TicketQuery>()
            .AddMutationType<TicketMutation>()
            .AddType<TicketType>();

        var app = builder.Build();
        await app.Services.ApplyMigrationsIfNeeded<TicketsDbContext>();

        app.MapGraphQL("/tickets");

        app.Run();

    }
}
