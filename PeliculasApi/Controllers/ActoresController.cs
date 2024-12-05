using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PeliculasApi.DTOs.Request;
using PeliculasApi.Entidades;

namespace PeliculasApi.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private const string cacheTag = "actores";

        public ActoresController(ApplicationDbContext applicationDbContext, IMapper mapper, IOutputCacheStore outputCacheStore)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
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
            applicationDbContext.Add(actor);
            await applicationDbContext.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return CreatedAtRoute("ObtenerActorPorId", new { id = actor.Id }, actor);
        }
    }
}
