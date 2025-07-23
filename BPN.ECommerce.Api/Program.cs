using BPN.ECommerce.Api.EndpointMappings;
using BPN.ECommerce.Api.Middlewares;
using BPN.ECommerce.Infrastructure.ServiceRegistrations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var redisSettings = builder.Configuration.GetSection("Redis");
var redisConnectionString = $"{redisSettings["BaseAddress"]}:{redisSettings["Port"]}";

builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString, name: "redis");
builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterHttpClient(builder.Configuration);
builder.Services.RegisterServices();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.RegisterProductEndpoints();
app.RegisterOrderEndpoints();

app.MapHealthChecks("/health");
app.Run();