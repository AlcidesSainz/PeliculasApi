using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Pelicula : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public required string Titulo { get; set; }
        public string? Trailer { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        [Unicode(false)]
        public string? Poster { get; set; }

        public string? Sinopsis { get; set; }

        public List<PeliculaActor> PeliculaActor { get; set; } = new List<PeliculaActor>();
        public List<PeliculaCine> PeliculaCine { get; set; } = new List<PeliculaCine>();
        public List<PeliculaGenero> PeliculaGenero { get; set; } = new List<PeliculaGenero>();


    }
}
