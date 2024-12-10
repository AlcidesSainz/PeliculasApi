using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Request
{
    public class CineRequestDTO
    {
        [Required]
        [StringLength(70)]
        public required string Nombre { get; set; }
        [Range(-90,90)]
        public double Latitud { get; set; }
        [Range(-180,180)]
        public double Longitud { get; set; }
    }
}
