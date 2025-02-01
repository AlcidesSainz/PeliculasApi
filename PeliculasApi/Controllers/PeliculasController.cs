using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs.PostGetModels;
using PeliculasApi.DTOs.PutGetModels;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;
using PeliculasApi.Utilities;


namespace PeliculasApi.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeliculasController : CustomBaseController
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IServicioUsuarios servicioUsuarios;
        private const string cacheTag = "peliculas";
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext dbContext, IMapper mapper, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos, IServicioUsuarios servicioUsuarios)
            : base(dbContext, mapper, outputCacheStore, cacheTag)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
            this.servicioUsuarios = servicioUsuarios;
        }
        [HttpGet("landing")]
        [OutputCache(Tags = [cacheTag])]
        [AllowAnonymous]
        public async Task<ActionResult<LandingPageResponseDTO>> Get()
        {
            var top = 6;
            var hoy = DateTime.Today;

            var proximosEstrenos = await dbContext.Peliculas
                .Where(p => p.FechaLanzamiento > hoy)
                .OrderBy(p => p.FechaLanzamiento)
                .Take(top)
                .ProjectTo<PeliculaResponseDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var enCines = await dbContext.Peliculas
                .Where(p => p.PeliculaCine.Select(pc => pc.PeliculaId).Contains(p.Id))
                .OrderBy(p => p.FechaLanzamiento)
                .Take(top)
                .ProjectTo<PeliculaResponseDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var todasPeliculas = await dbContext.Peliculas.Take(top).ProjectTo<PeliculaResponseDTO>(mapper.ConfigurationProvider).OrderByDescending(p=>p.Id).ToListAsync();

            var resultado = new LandingPageResponseDTO();
            resultado.EnCines = enCines;
            resultado.ProximosEstrenos = proximosEstrenos;
            resultado.TodasPeliculas = todasPeliculas;
            return resultado;
        }

        [HttpGet("{id:int}", Name = "ObtenerPeliculaPorId")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculasDetallesResponseDTO>> Get(int id)
        {
            var pelicula = await dbContext.Peliculas
                 .Include(p => p.PeliculaDirector)      // Incluye la relación principal
                 .ThenInclude(pd => pd.Actor)           // Asegúrate de cargar los actores
                 .ProjectTo<PeliculasDetallesResponseDTO>(mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            var promedioVoto = 0.0;
            var usuarioVoto = 0;

            if (await dbContext.RatingsPeliculas.AnyAsync(r => r.PeliculaId == id))
            {
                promedioVoto = await dbContext.RatingsPeliculas.Where(r => r.PeliculaId == id).AverageAsync(r => r.Puntuacion);

                if (HttpContext.User.Identity!.IsAuthenticated)
                {
                    var usuarioId = await servicioUsuarios.ObtenerUsuarioId();

                    var ratingDB = await dbContext.RatingsPeliculas.FirstOrDefaultAsync(r => r.PeliculaId == id && r.UsuarioId == usuarioId);

                    if(ratingDB is not null)
                    {
                        usuarioVoto = ratingDB.Puntuacion;
                    }
                }
            }
            pelicula.VotoPromedio = promedioVoto;
            pelicula.VotoUsuario = usuarioVoto;

            return pelicula;
        }

        [HttpGet("filtrar")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PeliculaResponseDTO>>> Filtrar([FromQuery] PeliculasFiltrarResponseDTO peliculasFiltrarResponseDTO)
        {
            var peliculaQueryable = dbContext.Peliculas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(peliculasFiltrarResponseDTO.Titulo))
            {
                peliculaQueryable = peliculaQueryable.Where(p => p.Titulo.Contains(peliculasFiltrarResponseDTO.Titulo));
            }
            if (peliculasFiltrarResponseDTO.EnCines)
            {
                peliculaQueryable = peliculaQueryable.Where(p => p.PeliculaCine.Select(pc => pc.PeliculaId).Contains(p.Id));
            }
            if (peliculasFiltrarResponseDTO.ProximoEstreno)
            {
                var hoy = DateTime.Today;
                peliculaQueryable = peliculaQueryable.Where(p => p.FechaLanzamiento > hoy);
            }
            if (peliculasFiltrarResponseDTO.GeneroId != 0)
            {
                peliculaQueryable = peliculaQueryable.Where(p => p.PeliculaGenero.Select(pg => pg.GeneroId).Contains(peliculasFiltrarResponseDTO.GeneroId));
            }
            await HttpContext.InsertarParametrosPaginacionEnCabecera(peliculaQueryable);

            var peliculas = await peliculaQueryable
                .Paginar(peliculasFiltrarResponseDTO.Paginacion)
                .ProjectTo<PeliculaResponseDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return peliculas;
        }

        [HttpGet("PostGet")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<PeliculasPostGetDTO>> PostGet()
        {
            var cines = await dbContext.Cines.ProjectTo<CineResponseDTO>(mapper.ConfigurationProvider).ToListAsync();

            var generos = await dbContext.Generos.ProjectTo<GeneroResponseDTO>(mapper.ConfigurationProvider).ToListAsync();

            return new PeliculasPostGetDTO
            {
                Cine = cines,
                Generos = generos
            };
        }
        [HttpGet("PutGet/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<PeliculasPutGetDTO>> PutGet(int id)
        {
            var peliculas = await dbContext.Peliculas.ProjectTo<PeliculasDetallesResponseDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == id);
            if (peliculas == null)
            {
                return NotFound();
            }


            var generosSeleccionados = peliculas.Generos.Select(g => g.Id).ToList();
            var generosNoSeleccionados = await dbContext.Generos
                                        .Where(g => !generosSeleccionados.Contains(g.Id))
                                        .ProjectTo<GeneroResponseDTO>(mapper.ConfigurationProvider)
                                        .ToListAsync();

            var cinesSeleccionados = peliculas.Cines.Select(c => c.Id).ToList();
            var cinesNoSeleccionados = await dbContext.Cines
                                    .Where(c => !cinesSeleccionados
                                    .Contains(c.Id))
                                    .ProjectTo<CineResponseDTO>(mapper.ConfigurationProvider)
                                    .ToListAsync();

            var respuesta = new PeliculasPutGetDTO();
            respuesta.Pelicula = peliculas;
            respuesta.GenerosSeleccionados = peliculas.Generos;
            respuesta.GenerosNoSeleccionados = generosNoSeleccionados;
            respuesta.CinesNoSeleccionados = cinesNoSeleccionados;
            respuesta.CinesSeleccionados = peliculas.Cines;
            respuesta.Actores = peliculas.Actores;
            respuesta.Directores = peliculas.Directores;
            return respuesta;

        }
        [HttpPut("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> Put(int id, [FromForm] PeliculaRequestDTO peliculaRequestDTO)
        {
            var pelicula = await dbContext.Peliculas
                                            .Include(c => c.PeliculaActor)
                                            .Include(c => c.PeliculaGenero)
                                            .Include(c => c.PeliculaCine)
                                            .Include(c=>c.PeliculaDirector)
                                            .FirstOrDefaultAsync(c => c.Id == id);
            if (pelicula is null)
            {
                return NotFound();
            }
            pelicula = mapper.Map(peliculaRequestDTO, pelicula);

            if (peliculaRequestDTO.Poster is not null)
            {
                pelicula.Poster = await almacenadorArchivos.Editar(pelicula.Poster, contenedor, peliculaRequestDTO.Poster);
            }
            AsignarOrdenActores(pelicula);

            await dbContext.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] PeliculaRequestDTO peliculaRequestDTO)
        {
            var peliculas = mapper.Map<Pelicula>(peliculaRequestDTO);
            if (peliculaRequestDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, peliculaRequestDTO.Poster);
                peliculas.Poster = url;
            }
            AsignarOrdenActores(peliculas);
            dbContext.Add(peliculas);
            await dbContext.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var peliculaDTO = mapper.Map<PeliculaResponseDTO>(peliculas);
            return CreatedAtRoute("ObtenerPeliculaPorId", new { id = peliculas.Id }, peliculaRequestDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Pelicula>(id);
        }
        private void AsignarOrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculaActor is not null)
            {
                for (int i = 0; i < pelicula.PeliculaActor.Count; i++)
                {
                    pelicula.PeliculaActor[i].Orden = 1;
                }
            }
        }
    }
}
