using System.Reflection;
using Azure.Messaging.ServiceBus;
using MediatR;
using MicroInventory.Category.Api.Domain.Repositories; // Ensure this namespace is included  
using MicroInventory.Category.Api.Domain.Repositories.Abstractions;
using MicroInventory.Category.Api.Domain.Repositories.EntityFramework;
using MicroInventory.Category.Api.Domain.Repositories.EntityFramework.DbContexts;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.EventBus;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using MicroInventory.Shared.EventBus.SubscriptionManagers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CategoryDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));




builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new ServiceBusClient(config["AzureServiceBus:ConnectionString"]);
});
builder.Services.AddSingleton<IEventBusSubscriptionManager>(sp =>
    new InMemoryEventBusSubscriptionManager(eventName => eventName));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddSingleton<IEventBus, AzureServiceBusEventBus>();






var app = builder.Build();

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
