using CHRISTIANEXAMENFINAL.Data;
using CHRISTIANEXAMENFINAL.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddScoped<EventosRepository>();
builder.WebHost.UseUrls("https://localhost:7275");

// Registro del contexto de base de datos con la cadena de conexi�n
builder.Services.AddDbContext<EventContext>(options =>
    options.UseSqlServer(connectionString));

// Configurar CORS para permitir solicitudes desde cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()   // Permitir cualquier origen (puerto/dominio)
                   .AllowAnyMethod()   // Permitir cualquier m�todo (GET, POST, PUT, DELETE, etc.)
                   .AllowAnyHeader();  // Permitir cualquier encabezado
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuraci�n del pipeline de solicitud HTTP.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

// Aplicar la pol�tica de CORS definida anteriormente
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
