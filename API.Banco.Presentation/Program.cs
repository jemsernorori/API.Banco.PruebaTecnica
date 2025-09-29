using API.Banco.Application.UsesCases.Clientes;
using API.Banco.Application.UsesCases.Cuentas;
using API.Banco.Application.UsesCases.Transacciones;
using API.Banco.Domain.Interfaces;
using API.Banco.Infrastructure.Repositories;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

//Handler
builder.Services.AddScoped<ClienteCrearHandler>();
builder.Services.AddScoped<ClienteObtenerHandler>();
builder.Services.AddScoped<CuentaCrearHandler>();
builder.Services.AddScoped<CuentaObtenerHandler>();
builder.Services.AddScoped<CuentaAplicarInteresHandler>();
builder.Services.AddScoped<TransaccionCrearHandler>();
builder.Services.AddScoped<TransaccionResumenHandler>();

// Inicializa SQLite
Batteries.Init();

//Repository
builder.Services.AddScoped<IConnectionManager, ConnectionManager>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();


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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
