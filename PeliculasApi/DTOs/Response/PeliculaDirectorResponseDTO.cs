using PeliculasApi.Entidades;

namespace PeliculasApi.DTOs.Response
{
    public class PeliculaDirectorResponseDTO
    {
        public int ActorId { get; set; }
        public int PeliculaId { get; set; }
        public Actor? Actor { get; set; }
    }
}
