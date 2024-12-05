using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }

        [Unicode(false)]
        public string? Foto { get; set; }
    }
}
