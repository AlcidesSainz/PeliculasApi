using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Request
{
    public class ActoresRequestDTO
    {

        [Required]
        [StringLength(150)]
        public required string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public IFormFile? Foto { get; set; }
    }
}
