using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasApi;
using PeliculasApi.Servicios;
using PeliculasApi.Utilities;
using System.Text;

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
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();
//Configurando el Auto Mapper con Geometry Factory
builder.Services.AddSingleton(proveedor => new MapperConfiguration(configuracion =>
{
    var geometryFactory = proveedor.GetRequiredService<GeometryFactory>();
    configuracion.AddProfile(new AutoMapperProfiles(geometryFactory));
}).CreateMapper());

//Configuracion de Identity
builder.Services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<SignInManager<IdentityUser>>();

builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

//Configurando politicas de acceso (claims)
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("esadmin", politica => politica.RequireClaim("esadmin"));
});


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
