using MassTransit;
using Orchestration.Api.Consumers;
using Orchestration.Api.Features;
using Orchestration.Api.Services;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<PurchaseSagaOrchestrator>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TicketReservedEventConsumer>();
    x.AddConsumer<ReservationConfirmedEventConsumer>();
    x.AddConsumer<ReservationCanceledEventConsumer>();
    x.AddConsumer<PaymentSucceededEventConsumer>();
    x.AddConsumer<PaymentFailedEventConsumer>();

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
builder.Services.AddApplicationServices(typeof(Program).Assembly, addEventPublisher: false);
builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.MapStartSaga();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.Run();
