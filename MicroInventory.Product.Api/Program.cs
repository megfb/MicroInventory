using System.Reflection;
using System.Text;
using Azure.Messaging.ServiceBus;
using MicroInventory.Product.Api.Domain.Repositories;
using MicroInventory.Product.Api.Domain.Repositories.Abstractions;
using MicroInventory.Product.Api.Domain.Repositories.EntityFramework;
using MicroInventory.Product.Api.Domain.Repositories.EntityFramework.DbContexts;
using MicroInventory.Product.Api.IntegrationEvents.EventHandlers;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.EventBus;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;
using MicroInventory.Shared.EventBus.SubscriptionManagers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));



builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new ServiceBusClient(config["AzureServiceBus:ConnectionString"]);
});
builder.Services.AddSingleton<IEventBusSubscriptionManager>(sp =>
    new InMemoryEventBusSubscriptionManager(eventName => eventName));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IEventBus, AzureServiceBusEventBus>();
builder.Services.AddTransient<CategoryCreatedEventHandler>();
builder.Services.AddTransient<CategoryUpdatedEventHandler>();
builder.Services.AddTransient<CategoryDeletedEventHandler>();
builder.Services.AddHostedService<AzureServiceBusConsumer>();


var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettingsCommon>();

builder.Services.Configure<JwtSettingsCommon>(
    builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Inventory API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT token'ýnýzý giriniz: Bearer <token>",

        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)),

            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();


//uygulama ayaða kalktýðýnda abone oluyoruz
using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    eventBus.Subscribe<CategoryCreatedIntegrationEvent, CategoryCreatedEventHandler>(
           topicName: "category-events-topic",
        subscriptionName: "new-category-added-sub");
    eventBus.Subscribe<CategoryUpdatedIntegrationEvent, CategoryUpdatedEventHandler>(
    topicName: "category-events-topic",
    subscriptionName: "category-updated-sub");

    eventBus.Subscribe<CategoryDeletedIntegrationEvent, CategoryDeletedEventHandler>(
        topicName: "category-events-topic",
        subscriptionName: "category-deleted-sub");
}





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
