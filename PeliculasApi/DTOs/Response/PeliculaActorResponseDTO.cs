namespace PeliculasApi.DTOs.Response
{
    public class PeliculaActorResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Foto { get; set; }
        public string Personaje { get; set; } = null!;
    }
}
