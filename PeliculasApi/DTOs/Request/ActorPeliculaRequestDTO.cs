namespace PeliculasApi.DTOs.Request
{
    public class ActorPeliculaRequestDTO
    {
        public int Id { get; set; }
        public required string Personaje { get; set; }
    }
}
