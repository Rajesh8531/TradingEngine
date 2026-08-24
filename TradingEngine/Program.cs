using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TradingEngine.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.AddCustomSerilog();

// Add services to the container.

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCorrelationId();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
