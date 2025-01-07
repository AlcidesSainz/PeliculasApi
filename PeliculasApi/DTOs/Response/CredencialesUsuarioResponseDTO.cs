using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Response
{
    public class CredencialesUsuarioResponseDTO
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
