using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;
using PeliculasApi.Utilities;

namespace PeliculasApi.Controllers
{
    [Route("api/actores")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "esadmin")]
    public class ActoresController : CustomBaseController
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IServicioUsuarios servicioUsuarios;
        private const string cacheTag = "actores";
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDbContext applicationDbContext, IMapper mapper, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos, IServicioUsuarios servicioUsuarios)
            : base(applicationDbContext, mapper, outputCacheStore, cacheTag)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
            this.servicioUsuarios = servicioUsuarios;
        }
        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        [AllowAnonymous]
        public async Task<List<ActoresResponseDTO>> Get([FromQuery] PaginacionResponseDTO paginacionResponseDTO)
        {
            return await Get<Actor, ActoresResponseDTO>(paginacionResponseDTO, ordenarPor: a => a.Nombre);
        }

        [HttpGet("landing")]
        [OutputCache(Tags = [cacheTag])]
        [AllowAnonymous]
        public async Task<ActionResult<LandingPageActoresResponseDTO>> Get()
        {
            var top = 10;

            var todosActores = await applicationDbContext.Actores.OrderBy(a=>Guid.NewGuid()).Take(top).ProjectTo<ActoresResponseDTO>(mapper.ConfigurationProvider).ToListAsync();

            var resultado = new LandingPageActoresResponseDTO();
            resultado.EnTendencia = todosActores;
            return resultado;
        }


        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ActoresResponseDTO>>> Filtrar([FromQuery] ActoresFiltrarResponseDTO actoresFiltrarResponseDTO)
        {
            var actorQueryable = applicationDbContext.Actores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(actoresFiltrarResponseDTO.Nombre))
            {
                actorQueryable = actorQueryable.Where(p => p.Nombre.Contains(actoresFiltrarResponseDTO.Nombre));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(actorQueryable);

            var actores = await actorQueryable
                .Paginar(actoresFiltrarResponseDTO.Paginacion)
                .ProjectTo<ActoresResponseDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return actores;
        }

        [HttpGet("{id:int}", Name = "ObtenerActorPorId")]
        [OutputCache(Tags = [cacheTag])]
        [AllowAnonymous]
        public async Task<ActionResult<ActoresResponseDTO>> Get(int id)
        {
            var actor = await applicationDbContext.Actores
                 .ProjectTo<ActoresResponseDTO>(mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(p => p.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            var promedioVoto = 0.0;
            var usuarioVoto = 0;
            int ultimaPeliculaId ;
            int primeraPeliculaId;
            string? ultimaPelicula = null;
            string? primeraPelicula = null;

            ultimaPelicula = await applicationDbContext.Peliculas
                .Where(p => p.PeliculaActor.Any(pa => pa.ActorId == id))
                .OrderByDescending(p => p.FechaLanzamiento)
                .Select(p => p.Titulo) // Obtener solo el título
                .FirstOrDefaultAsync();

            ultimaPeliculaId = await applicationDbContext.Peliculas
                .Where(p => p.PeliculaActor.Any(pa => pa.ActorId == id))
                .OrderByDescending(p => p.FechaLanzamiento)
                .Select(p => p.Id) // Obtener solo el Id
                .FirstOrDefaultAsync();

            primeraPelicula = await applicationDbContext.Peliculas
                .Where(p => p.PeliculaActor.Any(pa => pa.ActorId == id))
                .OrderBy(p => p.FechaLanzamiento)
                .Select(p => p.Titulo) // Obtener solo el título
                .FirstOrDefaultAsync();
            primeraPeliculaId = await applicationDbContext.Peliculas
                .Where(p => p.PeliculaActor.Any(pa => pa.ActorId == id))
                .OrderBy(p => p.FechaLanzamiento)
                .Select(p => p.Id) // Obtener solo el ID
                .FirstOrDefaultAsync();

            // Obtener todas las películas del actor
            var peliculas = await applicationDbContext.Peliculas
                .Where(p => p.PeliculaActor.Any(pa => pa.ActorId == id)) // Filtrar por actor
                .OrderByDescending(p => p.FechaLanzamiento)
                .Select(p => new PeliculaActorListadoResponseDTO // Solo los datos necesarios
                {
                    Titulo = p.Titulo,
                    FechaLanzamiento = p.FechaLanzamiento,
                    Poster = p.Poster
                })
                .ToListAsync();


            if (await applicationDbContext.RatingActores.AnyAsync(r => r.ActorId == id))
            {
                promedioVoto = await applicationDbContext.RatingActores.Where(r => r.ActorId == id).AverageAsync(r => r.Puntuacion);

                if (HttpContext.User.Identity!.IsAuthenticated)
                {
                    var usuarioId = await servicioUsuarios.ObtenerUsuarioId();

                    var ratingDB = await applicationDbContext.RatingActores.FirstOrDefaultAsync(r => r.ActorId == id && r.UsuarioId == usuarioId);

                    if (ratingDB is not null)
                    {
                        usuarioVoto = ratingDB.Puntuacion;
                    }
                }
            }
            actor.VotoPromedio = promedioVoto;
            actor.VotoUsuario = usuarioVoto;
            actor.UltimaPelicula = ultimaPelicula;
            actor.PrimeraPelicula = primeraPelicula;
            actor.UltimaPeliculaId = ultimaPeliculaId;
            actor.PrimeraPeliculaId = primeraPeliculaId;


            return actor;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<PeliculaActorResponseDTO>>> Get(string nombre)
        {
            return await applicationDbContext.Actores.Where(a => a.Nombre.Contains(nombre))
                .ProjectTo<PeliculaActorResponseDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("{id:int}/ultima-pelicula")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaResponseDTO>> GetUltimaPelicula(int id)
        {
            var ultimaPelicula = await applicationDbContext.Peliculas
                .Where(p => p.PeliculaActor.Any(pa => pa.ActorId == id)) // Filtrar por el actor
                .OrderByDescending(p => p.FechaLanzamiento) // Ordenar por fecha de lanzamiento descendente
                .ProjectTo<PeliculaResponseDTO>(mapper.ConfigurationProvider) // Usar AutoMapper
                .FirstOrDefaultAsync();

            if (ultimaPelicula == null)
            {
                return NotFound($"No se encontraron películas para el actor con ID {id}.");
            }

            return Ok(ultimaPelicula);
        }
        [HttpGet("{id:int}/primera-pelicula")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculaResponseDTO>> GetPrimeraPelicula(int id)
        {
            var primera = await applicationDbContext.Peliculas
                .Where(p => p.PeliculaActor.Any(pa => pa.ActorId == id)) // Filtrar por el actor
                .OrderBy(p => p.FechaLanzamiento) // Ordenar por fecha de lanzamiento descendente
                .ProjectTo<PeliculaResponseDTO>(mapper.ConfigurationProvider) // Usar AutoMapper
                .FirstOrDefaultAsync();

            if (primera == null)
            {
                return NotFound($"No se encontraron películas para el actor con ID {id}.");
            }

            return Ok(primera);
        }
        [HttpGet("{id:int}/peliculas")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PeliculaResponseDTO>>> GetPeliculasPorActor(int id)
        {
            var peliculas = await applicationDbContext.Peliculas
                .Where(p => p.PeliculaActor.Any(pa => pa.ActorId == id)) // Filtrar por el actor
                .OrderByDescending(p => p.FechaLanzamiento) // Ordenar por fecha de lanzamiento descendente
                .ProjectTo<PeliculaResponseDTO>(mapper.ConfigurationProvider) // Mapear con AutoMapper
                .ToListAsync();

            if (peliculas == null || peliculas.Count == 0)
            {
                return NotFound($"No se encontraron películas para el actor con ID {id}.");
            }

            return Ok(peliculas);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ActoresRequestDTO actoresRequestDTO)
        {
            var actor = mapper.Map<Actor>(actoresRequestDTO);

            if (actoresRequestDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, actoresRequestDTO.Foto);
                actor.Foto = url;
            }

            applicationDbContext.Add(actor);
            await applicationDbContext.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return CreatedAtRoute("ObtenerActorPorId", new { id = actor.Id }, actor);
        }
        [HttpPut("{id:int}")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<IActionResult> Put(int id, [FromForm] ActoresRequestDTO actoresRequestDTO)
        {
            var actor = await applicationDbContext.Actores.FirstOrDefaultAsync(a => a.Id == id);
            if (actor is null)
            {
                return NotFound();
            }
            actor = mapper.Map(actoresRequestDTO, actor);
            if (actoresRequestDTO.Foto is not null)
            {
                actor.Foto = await almacenadorArchivos.Editar(actor.Foto, contenedor, actoresRequestDTO.Foto);
            }
            await applicationDbContext.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Actor>(id);
        }
    }
}
