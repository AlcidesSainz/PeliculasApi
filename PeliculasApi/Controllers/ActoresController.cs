using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PeliculasApi.DTOs.Request;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;

namespace PeliculasApi.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private const string cacheTag = "actores";
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDbContext applicationDbContext, IMapper mapper, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
        }


        [HttpGet("{id:int}", Name = "ObtenerActorPorId")]
        public void Get(int id)
        {
            throw new NotImplementedException();
        }

        //FromForm es para que se puedan enviar archivos en el formulario
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ActoresRequestDTO actoresRequestDTO)
        {
            var actor = mapper.Map<Actor>(actoresRequestDTO);

            if(actoresRequestDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, actoresRequestDTO.Foto);
                actor.Foto = url;
            }

            applicationDbContext.Add(actor);
            await applicationDbContext.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return CreatedAtRoute("ObtenerActorPorId", new { id = actor.Id }, actor);
        }
    }
}
