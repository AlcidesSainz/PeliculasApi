using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs.PostGetModels;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;


namespace PeliculasApi.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : CustomBaseController
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private const string cacheTag = "peliculas";
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext dbContext, IMapper mapper, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
            : base(dbContext, mapper, outputCacheStore, cacheTag)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet("{id:int}", Name = "ObtenerPeliculaPorId")]
        public Task<IActionResult> Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet("PostGet")]
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


        [HttpPost]
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
