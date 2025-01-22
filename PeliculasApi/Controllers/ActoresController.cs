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
        private const string cacheTag = "actores";
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDbContext applicationDbContext, IMapper mapper, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
            : base(applicationDbContext, mapper, outputCacheStore, cacheTag)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
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

            var todosActores = await applicationDbContext.Actores.Take(top).ProjectTo<ActoresResponseDTO>(mapper.ConfigurationProvider).ToListAsync();

            var resultado = new LandingPageActoresResponseDTO();
            resultado.EnTendencia = todosActores;
            return resultado;
        }

        [HttpGet("{id:int}", Name = "ObtenerActorPorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<ActoresResponseDTO>> Get(int id)
        {
            return await Get<Actor, ActoresResponseDTO>(id);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<PeliculaActorResponseDTO>>> Get(string nombre)
        {
            return await applicationDbContext.Actores.Where(a => a.Nombre.Contains(nombre))
                .ProjectTo<PeliculaActorResponseDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        //FromForm es para que se puedan enviar archivos en el formulario
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
