using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Request
{
    public class GeneroRequestDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(50, ErrorMessage = "El campo {0} no puede tener {1} caracteres")]
        [PrimeraLetraMayuscula]
        public required string Nombre { get; set; }
    }
}
