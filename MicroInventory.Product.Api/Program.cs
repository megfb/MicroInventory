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

builder.Services.AddSingleton<IEventBus, AzureServiceBusEventBus>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Integration Event Handler'lar
builder.Services.AddTransient<CategoryCreatedEventHandler>();
builder.Services.AddTransient<CategoryUpdatedEventHandler>();
builder.Services.AddTransient<CategoryDeletedEventHandler>();

// Hosted service (consumer)
builder.Services.AddHostedService<AzureServiceBusConsumer>();

// JWT Ayarlarý
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettingsCommon>();

builder.Services.Configure<JwtSettingsCommon>(
    builder.Configuration.GetSection("JwtSettings"));

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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Product API", Version = "v1" });

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

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

    eventBus.Subscribe<CategoryCreatedIntegrationEvent, CategoryCreatedEventHandler>();
    eventBus.Subscribe<CategoryUpdatedIntegrationEvent, CategoryUpdatedEventHandler>();
    eventBus.Subscribe<CategoryDeletedIntegrationEvent, CategoryDeletedEventHandler>();
}

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
