using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Cines:IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(70)]
        public required string Nombre { get; set; }
        public required Point Ubicacion { get; set; }
    }
}
