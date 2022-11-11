using Core.Infrastructure;
using MassTransit;
using Messages;
using Stock.API.Consumers;
using Stock.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dependencyOptions = new DependencyOptions
{
    AddMessageBroker = true,
    AddDistributedTracing = true,
    AddRedis = true,
    MessageBrokerConfiguration = x =>
    {
        x.AddConsumer<OrderCreatedConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(host: builder.Configuration.GetConnectionString("RabbitMq"), h =>
            {
                h.Username(builder.Configuration["RabbitMq:UserName"]);
                h.Password(builder.Configuration["RabbitMq:Password"]);
            });

            cfg.ReceiveEndpoint(RabbitMqConsts.OrderCreatedQueueName, e => {
                e.ConfigureConsumer<OrderCreatedConsumer>(context);
            });

        });
    }
};

builder.Services.AddInfrastructure(builder.Configuration, dependencyOptions);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
