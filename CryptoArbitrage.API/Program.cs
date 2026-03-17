using CryptoArbitrage.Infrastructure;
using CryptoArbitrage.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redireciona chamadas HTTP para HTTPS automaticamente
app.UseHttpsRedirection();

// Autorização
app.UseAuthorization();

// Mapeia as rotas para os métodos dentro das nossas Controllers
app.MapControllers();


app.Run();