using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Response
{
    public class ActoresResponseDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string? Foto { get; set; }
    }
}
