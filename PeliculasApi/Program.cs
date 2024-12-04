using Microsoft.EntityFrameworkCore;
using PeliculasApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configurando el cache que tiene un tiempo de expiration de 60 segundos, luego de ese tiempo todo lo almacenado en el cache se va a eliminar
builder.Services.AddOutputCache(opciones =>
{
    opciones.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
});

//Configurando el permiso de origen para acceder a la API WEB
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!.Split(",");

//Configurando el CORS 
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(opcionesCORS =>
    {
        opcionesCORS.WithOrigins(origenesPermitidos).AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("cantidad-total-registros");
    });
});



//Configurando el Auto Mapper
builder.Services.AddAutoMapper(typeof(Program));

//Configurando la conexion a la base de datos SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

//Para Poder utilizar el cache en nuestra app
app.UseOutputCache();

app.UseAuthorization();

app.MapControllers();

app.Run();