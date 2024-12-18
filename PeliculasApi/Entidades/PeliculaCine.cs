namespace PeliculasApi.Entidades
{
    public class PeliculaCine
    {
        public int CineId { get; set; }
        public int PeliculaId { get; set; }
        public Cines Cines { get; set; } = null!;
        public Pelicula Pelicula { get; set; } = null!;
    }
}
