using PeliculasApi.DTOs.Response;

namespace PeliculasApi.DTOs.PostGetModels
{
    public class PeliculasPostGetDTO
    {
        public List <GeneroResponseDTO> Generos { get; set; } = new List<GeneroResponseDTO> ();

        public List <CineResponseDTO> Cine { get; set; } = new List<CineResponseDTO> ();
    }
}
