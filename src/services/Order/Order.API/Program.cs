using Core.Infrastructure;
using MassTransit;
using Order.Application;
using Order.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Add services to the container.
var dependencyOptions = new DependencyOptions
{
    AddHttpClient = true,
    AddMessageBroker = true,
    MessageBrokerConfiguration = x =>
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(host: builder.Configuration.GetConnectionString("RabbitMq"), h =>
            {
                h.Username(builder.Configuration["RabbitMq:UserName"]);
                h.Password(builder.Configuration["RabbitMq:Password"]);
            });

        });
    }
};

builder.Services.AddApplication();
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
