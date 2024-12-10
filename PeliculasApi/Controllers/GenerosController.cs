using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PeliculasApi.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : CustomBaseController
    {

        private readonly IOutputCacheStore outputCacheStore;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private const string cacheTag = "generos";

        public GenerosController(IOutputCacheStore outputCacheStore, ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper, outputCacheStore, cacheTag)
        {
            this.outputCacheStore = outputCacheStore;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<GeneroResponseDTO>> Get([FromQuery] PaginacionResponseDTO paginacionResponseDTO)
        {
            return await Get<Genero, GeneroResponseDTO>(paginacionResponseDTO, ordenarPor: g => g.Nombre);
        }

        [HttpGet("{id:int}", Name = "ObtenerGeneroPorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<GeneroResponseDTO>> Get(int id)
        {
            return await Get<Genero, GeneroResponseDTO>(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GeneroRequestDTO generoRequestDTO)
        {
            return await Post<GeneroRequestDTO, Genero, GeneroResponseDTO>(generoRequestDTO, "ObtenerGeneroPorId");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] GeneroRequestDTO generoRequestDTO)
        {
            return await Put<GeneroRequestDTO, Genero>(id, generoRequestDTO);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Genero>(id);
        }
    }
}
