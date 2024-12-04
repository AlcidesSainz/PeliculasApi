using AutoMapper;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;

namespace PeliculasApi.Utilities
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            ConfigureMapperGeneros();
        }
        private void ConfigureMapperGeneros()
        {
            CreateMap<GeneroRequestDTO, Genero>();
            CreateMap<GeneroResponseDTO, Genero>().ReverseMap();
        }
    }

}
