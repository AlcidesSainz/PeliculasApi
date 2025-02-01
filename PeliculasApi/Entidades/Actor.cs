﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Actor :IId
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public required string Nombre { get; set; }

        public DateTime FechaNacimiento { get; set; }
        [StringLength(2000)]
        public string? Biografia { get; set; }

        [Unicode(false)]
        public string? Foto { get; set; }
    }
}
