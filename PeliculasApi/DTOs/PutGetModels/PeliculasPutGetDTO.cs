using PeliculasApi.DTOs.Response;

namespace PeliculasApi.DTOs.PutGetModels
{
    public class PeliculasPutGetDTO
    {
        public PeliculaResponseDTO Pelicula { get; set; } = null!;
        public List<GeneroResponseDTO> GenerosSeleccionados { get; set; } = new List<GeneroResponseDTO>();
        public List<GeneroResponseDTO> GenerosNoSeleccionados { get; set; } = new List<GeneroResponseDTO>();
        public List<CineResponseDTO> CinesSeleccionados { get; set; } = new List<CineResponseDTO>();
        public List<CineResponseDTO> CinesNoSeleccionados { get; set; } = new List<CineResponseDTO>();
        public List<PeliculaActorResponseDTO> Actores { get; set; } = new List<PeliculaActorResponseDTO>();
    }
}
