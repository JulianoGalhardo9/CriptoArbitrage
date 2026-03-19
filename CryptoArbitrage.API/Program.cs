using CryptoArbitrage.Infrastructure;
using CryptoArbitrage.Application;
using CryptoArbitrage.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CryptoArbitrage.Application.Workers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8080");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/arbitrage-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddHostedService<ArbitrageMonitorWorker>();

var app = builder.Build();

app.UseRouting(); 
app.UseCors("AllowAngular"); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var db = services.GetRequiredService<AppDbContext>();
        var retries = 10;
        while (retries > 0)
        {
            try { db.Database.Migrate(); break; }
            catch { retries--; Thread.Sleep(3000); }
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Erro ao aplicar migrations.");
    }
}

app.Run();