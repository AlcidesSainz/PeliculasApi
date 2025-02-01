using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class PeliculaDirector
    {
        public int ActorId { get; set; }
        public int PeliculaId { get; set; }
        public Actor Actor { get; set; } = null!;
        public Pelicula Pelicula { get; set; } = null!;
    }
}
