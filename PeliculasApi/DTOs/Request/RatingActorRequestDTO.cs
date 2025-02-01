using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Request
{
    public class RatingActorRequestDTO
    {
        public int ActorId { get; set; }
        [Range(1, 10)]
        public int Puntuacion { get; set; }
    }
}
