namespace PeliculasApi.DTOs.Response
{
    public class RespuestaAutenticacionResponseDTO
    {
        public required string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
