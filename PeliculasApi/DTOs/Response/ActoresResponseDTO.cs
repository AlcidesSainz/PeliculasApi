using Microsoft.EntityFrameworkCore;
using PeliculasApi.Entidades;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Response
{
    public class ActoresResponseDTO :IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }
        [StringLength(2000)]
        public string? Biografia { get; set; }

        public string? Foto { get; set; }
        public double VotoPromedio { get; set; }
        public int VotoUsuario { get; set; }
        public string PrimeraPelicula { get; set; }
        public string UltimaPelicula { get; set; }
        public int UltimaPeliculaId { get; set; }
        public int PrimeraPeliculaId { get; set; }
        public List<PeliculaActorListadoResponseDTO> Peliculas { get; set; } = new();

    }
}
