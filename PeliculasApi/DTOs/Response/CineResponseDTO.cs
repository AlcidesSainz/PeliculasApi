using NetTopologySuite.Geometries;
using PeliculasApi.Entidades;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.DTOs.Response
{
    public class CineResponseDTO : IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public double Latidad { get; set; }
        public double Longitud { get; set; }
    }
}
