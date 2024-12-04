using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Response
{
    public class GeneroResponseDTO
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
    }
}
