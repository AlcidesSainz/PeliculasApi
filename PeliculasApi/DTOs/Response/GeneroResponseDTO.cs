using PeliculasApi.Entidades;
using PeliculasApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Response
{
    public class GeneroResponseDTO:IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
    }
}
