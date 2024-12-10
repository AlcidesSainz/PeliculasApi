using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasApi;
using PeliculasApi.Servicios;
using PeliculasApi.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//COnfiguracion para poder utilizar Geometry Factory para la ubicacion 
builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

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

builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorArchivoLocal>();
builder.Services.AddHttpContextAccessor();
//Configurando el Auto Mapper con Geometry Factory
builder.Services.AddSingleton(proveedor => new MapperConfiguration(configuracion =>
{
    var geometryFactory = proveedor.GetRequiredService<GeometryFactory>();
    configuracion.AddProfile(new AutoMapperProfiles(geometryFactory));
}).CreateMapper());

//Configurando la conexion a la base de datos SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
opciones.UseSqlServer("name=DefaultConnection", sqlServer => sqlServer.UseNetTopologySuite()));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

//Para Poder utilizar el cache en nuestra app
app.UseOutputCache();

app.Run();
