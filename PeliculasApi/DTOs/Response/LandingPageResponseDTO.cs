namespace PeliculasApi.DTOs.Response
{
    public class LandingPageResponseDTO
    {
        public List<PeliculaResponseDTO> EnCines { get; set; } = new List<PeliculaResponseDTO>();
        public List<PeliculaResponseDTO> ProximosEstrenos { get; set; } = new List<PeliculaResponseDTO>();

        public List<PeliculaResponseDTO> TodasPeliculas { get; set; } = new List<PeliculaResponseDTO>();
    }
}
