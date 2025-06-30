using System.Reflection;
using Azure.Messaging.ServiceBus;
using MicroInventory.Assignment.Api.Domain.Repositories;
using MicroInventory.Assignment.Api.Domain.Repositories.Abstractions;
using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork;
using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts;
using MicroInventory.Assignment.Api.Application.IntegrationEvents.EventHandlers;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.EventBus;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using MicroInventory.Shared.EventBus.SubscriptionManagers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AssignmentDbContext>(options =>
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
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Integration Event Handler'lar
builder.Services.AddTransient<ProductAddedEventHandler>();
builder.Services.AddTransient<ProductUpdatedEventHandler>();
builder.Services.AddTransient<ProductDeletedEventHandler>();
builder.Services.AddTransient<PersonAddedEventHandler>();
builder.Services.AddTransient<PersonUpdatedEventHandler>();
builder.Services.AddTransient<PersonDeletedEventHandler>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

    eventBus.Subscribe<ProductAddedIntegrationEvent, ProductAddedEventHandler>();
    eventBus.Subscribe<ProductUpdatedIntegrationEvent, ProductUpdatedEventHandler>();
    eventBus.Subscribe<ProductDeletedIntegrationEvent, ProductDeletedEventHandler>();
    eventBus.Subscribe<PersonAddedIntegrationEvent, PersonAddedEventHandler>();
    eventBus.Subscribe<PersonUpdatedIntegrationEvent, PersonUpdatedEventHandler>();
    eventBus.Subscribe<PersonDeletedIntegrationEvent, PersonDeletedEventHandler>();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
