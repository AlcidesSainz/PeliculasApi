using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;
using PeliculasApi.Servicios;

namespace PeliculasApi.Controllers
{
    [ApiController]
    [Route("api/ratings")]
    public class RatingsController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IServicioUsuarios servicioUsuarios;

        public RatingsController(ApplicationDbContext applicationDbContext, IServicioUsuarios servicioUsuarios)
        {
            this.applicationDbContext = applicationDbContext;
            this.servicioUsuarios = servicioUsuarios;
        }
        [HttpGet("ranking-actores")]
        public async Task<ActionResult<List<ActoresResponseDTO>>> GetRankingActores()
        {
            var actores = await applicationDbContext.Actores
                .Select(a => new ActoresResponseDTO
                {
                    Id = a.Id,
                    Nombre = a.Nombre,
                    FechaNacimiento = a.FechaNacimiento,
                    Foto = a.Foto,
                    VotoPromedio = applicationDbContext.RatingActores
                        .Where(r => r.ActorId == a.Id)
                        .Average(r => (double?)r.Puntuacion) ?? 0  // Calcula el promedio o 0 si no tiene votos
                })
                .OrderByDescending(a => a.VotoPromedio) // Ordenar de mayor a menor
                .ToListAsync();

            return Ok(actores);
        }
        [HttpGet("ranking-peliculas")]
        public async Task<ActionResult<List<PeliculaResponseDTO>>> GetRankingPeliculas()
        {
            var peliculas = await applicationDbContext.Peliculas
                .Select(a => new PeliculaResponseDTO
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    FechaLanzamiento = a.FechaLanzamiento,
                    Poster = a.Poster,
                    VotoPromedio = applicationDbContext.RatingsPeliculas
                        .Where(r => r.PeliculaId == a.Id)
                        .Average(r => (double?)r.Puntuacion) ?? 0  // Calcula el promedio o 0 si no tiene votos
                })
                .OrderByDescending(a => a.VotoPromedio) // Ordenar de mayor a menor
                .ToListAsync();

            return Ok(peliculas);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] RatingRequestDTO ratingRequestDTO)
        {
            var usuarioId = await servicioUsuarios.ObtenerUsuarioId();

            var ratingActual = await applicationDbContext.RatingsPeliculas
                .FirstOrDefaultAsync(x => x.PeliculaId == ratingRequestDTO.PeliculaId && x.UsuarioId == usuarioId);

            if (ratingActual == null)
            {
                var rating = new Rating()
                {
                    PeliculaId = ratingRequestDTO.PeliculaId,
                    Puntuacion = ratingRequestDTO.Puntuacion,
                    UsuarioId = usuarioId
                };
                applicationDbContext.Add(rating);
            }
            else
            {
                ratingActual.Puntuacion = ratingRequestDTO.Puntuacion;
            }
            await applicationDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("actor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Post([FromBody] RatingActorRequestDTO ratingRequestDTO)
        {
            var usuarioId = await servicioUsuarios.ObtenerUsuarioId();

            var ratingActual = await applicationDbContext.RatingActores
                .FirstOrDefaultAsync(x => x.ActorId == ratingRequestDTO.ActorId && x.UsuarioId == usuarioId);

            if (ratingActual == null)
            {
                var rating = new RatingActor()
                {
                    ActorId = ratingRequestDTO.ActorId,
                    Puntuacion = ratingRequestDTO.Puntuacion,
                    UsuarioId = usuarioId
                };
                applicationDbContext.Add(rating);
            }
            else
            {
                ratingActual.Puntuacion = ratingRequestDTO.Puntuacion;
            }
            await applicationDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
