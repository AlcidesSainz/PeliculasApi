using Microsoft.AspNetCore.Identity;

namespace PeliculasApi.Entidades
{
    public class RatingActor
    {
        public int Id { get; set; }
        public int Puntuacion { get; set; }
        public int ActorId { get; set; }
        public required string UsuarioId { get; set; }
        public Actor Actor { get; set; } = null!;
        public IdentityUser Usuario { get; set; } = null!;
    }
}
