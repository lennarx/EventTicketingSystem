using MassTransit;
using Payments.Api.Consumers;
using Payments.Api.Services;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProcessPaymentCommandConsumer>();
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

builder.Services.AddApplicationServices(typeof(Program).Assembly, addEventPublisher: false, addMediaTr: false);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();
