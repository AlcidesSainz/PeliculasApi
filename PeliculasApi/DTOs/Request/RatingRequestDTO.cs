using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Request
{
    public class RatingRequestDTO
    {
        public int PeliculaId { get; set; }
        [Range(1, 10)]
        public int Puntuacion { get; set; }
    }
}
