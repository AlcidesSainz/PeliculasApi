using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;
using PeliculasApi.Utilities;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace PeliculasApi.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {

        private readonly IOutputCacheStore outputCacheStore;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private const string cacheTag = "generos";

        public GenerosController(IOutputCacheStore outputCacheStore, ApplicationDbContext dbContext, IMapper mapper)
        {
            this.outputCacheStore = outputCacheStore;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet]
        //[OutputCache(Tags = [cacheTag])]
        public async Task<List<GeneroResponseDTO>> Get([FromQuery] PaginacionResponseDTO paginacionResponseDTO)
        {

            var queryable = dbContext.Generos;
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable
                .OrderBy(g => g.Nombre)
                .Paginar(paginacionResponseDTO)
                .ProjectTo<GeneroResponseDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        //[HttpGet("{id:int}", Name = "ObtenerGeneroPorId")]
        //[OutputCache(Tags = [cacheTag])]
        //public async Task<ActionResult<GeneroResponseDTO>> Get(int id)
        //{
        //    throw new NotImplementedException();
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GeneroRequestDTO generoRequestDTO)
        {
            var genero = mapper.Map<Genero>(generoRequestDTO);
            dbContext.Add(genero);
            await dbContext.SaveChangesAsync();
            return CreatedAtRoute("ObtenerGeneroPorId", new { id = genero.Id }, genero);
        }
        [HttpPut]
        public void Put() { }
        [HttpDelete]
        public void Delete() { }
    }
}
