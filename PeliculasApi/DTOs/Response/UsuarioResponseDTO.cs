namespace PeliculasApi.DTOs.Response
{
    public class UsuarioResponseDTO
    {
        public required string Email { get; set; } = string.Empty;
        public bool EsAdmin { get; set; }
    }
}
