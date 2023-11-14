using BazarAPI.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configura el contexto de la base de datos
builder.Services.AddDbContext<BazarContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Aseg�rate de que el nombre coincida con tu appsettings.json

// Configuraci�n de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowEverything",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aplicar la pol�tica de CORS
app.UseCors("AllowEverything");

app.UseAuthorization();

app.MapControllers();

app.Run();
