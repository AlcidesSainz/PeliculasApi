namespace PeliculasApi.DTOs.Response
{
    public class PeliculasDetallesResponseDTO: PeliculaResponseDTO
    {
        public List<GeneroResponseDTO> Generos { get; set; } = new List<GeneroResponseDTO>();
        public List<CineResponseDTO> Cines { get; set; } = new List<CineResponseDTO>();
        public List<PeliculaActorResponseDTO> Actores { get; set; } = new List<PeliculaActorResponseDTO>();
        public List<PeliculaDirectorResponseDTO> Directores { get; set; } = new List<PeliculaDirectorResponseDTO>();
    }
}
