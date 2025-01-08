using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs.Request;
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
    }
}
