namespace PeliculasApi.DTOs.Response
{
    public class ActorDetallesResponseDTO
    {
        public List<PeliculaActorResponseDTO> Actores { get; set; } = new List<PeliculaActorResponseDTO>();

    }
}
