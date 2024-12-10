using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Genero:IId
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="El campo {0} es requerido")]
        [StringLength(50,ErrorMessage ="El campo {0} no puede tener {1} caracteres")]
        [PrimeraLetraMayuscula]
        public required string Nombre { get; set; }

    }
}
