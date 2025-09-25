using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using HamburguerApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON (enum como string, ignora ciclos se aparecer)
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// CORS liberadão (ajuste depois se precisar)
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hamburguer API",
        Version = "v1",
        Description = "API de Lanchonete – .NET 8 + EF Core + SQL Server"
    });

    // XML comments (habilitar no .csproj — veja abaixo)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    // Enums como string no schema
    c.DescribeAllParametersInCamelCase();
});

// EF Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
// Se seus validators estão no mesmo assembly do Program, esta linha funciona.
// Se quiser garantir, aponte para um validator concreto:
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// Exemplo robusto:
// builder.Services.AddValidatorsFromAssemblyContaining<HamburguerApi.Validators.BebidaCreateValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Se reclamar de HTTPS, deixe comentado mesmo
// app.UseHttpsRedirection();

app.UseStaticFiles(); // serve wwwroot/index.html
app.UseCors();
app.MapControllers();

app.Run();
