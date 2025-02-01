using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.Utilities;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Request
{
    public class PeliculaRequestDTO
    {
        [Required]
        [StringLength(300)]
        public required string Titulo { get; set; }
        public string? Trailer { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public IFormFile? Poster { get; set; }

        public string? Sinopsis { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder))]
        public List<int>? CinesIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder))]
        public List<int>? GenerosIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder))]
        public List<ActorPeliculaRequestDTO>? Actores { get; set; }

        [ModelBinder(BinderType =typeof(TypeBinder))]
        public List<PeliculaDirectorRequestDTO>? Directores { get; set; }
    }
}
