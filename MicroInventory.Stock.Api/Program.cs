using Azure.Messaging.ServiceBus;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.SubscriptionManagers;
using MicroInventory.Shared.EventBus;
using MicroInventory.Stock.Api.Domain.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Stock.Api.Domain.Repositories.Abstractions;
using MicroInventory.Stock.Api.Domain.Repositories.EntityFramework;
using MicroInventory.Stock.Api.Domain.Repositories;
using MicroInventory.Stock.Api.Application.IntegrationEvents.EventHandlers;
using MicroInventory.Shared.EventBus.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StockDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));

// Azure Service Bus client
builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new ServiceBusClient(config["AzureServiceBus:ConnectionString"]);
});

// Event Bus ve handler'lar
builder.Services.AddSingleton<IEventBusSubscriptionManager>(
    sp => new InMemoryEventBusSubscriptionManager(eventName => eventName));
builder.Services.AddSingleton<IEventBus, AzureServiceBusEventBus>();
builder.Services.AddHostedService<AzureServiceBusConsumer>();

// Repository & UoW
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Integration Event Handler'lar
builder.Services.AddTransient<ProductAddedEventHandler>();
builder.Services.AddTransient<ProductUpdatedEventHandler>();
builder.Services.AddTransient<ProductDeletedEventHandler>();
builder.Services.AddTransient<AssignmentAddedEventHandler>();
builder.Services.AddTransient<AssignmentDeletedEventHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

    eventBus.Subscribe<ProductAddedIntegrationEvent, ProductAddedEventHandler>();
    eventBus.Subscribe<ProductUpdatedIntegrationEvent, ProductUpdatedEventHandler>();
    eventBus.Subscribe<ProductDeletedIntegrationEvent, ProductDeletedEventHandler>();
    eventBus.Subscribe<AssignmentAddedIntegrationEvent, AssignmentAddedEventHandler>();
    eventBus.Subscribe<AssignmentDeletedIntegrationEvent, AssignmentDeletedEventHandler>();

}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
