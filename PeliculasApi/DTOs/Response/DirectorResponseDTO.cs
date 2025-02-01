namespace PeliculasApi.DTOs.Response
{
    public class DirectorResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Foto { get; set; }
    }
}
