using AutoMapper;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;

namespace PeliculasApi.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            ConfigureMapperGeneros();
            ConfigureMapperActores();
        }
        private void ConfigureMapperGeneros()
        {
            CreateMap<GeneroRequestDTO, Genero>();
            CreateMap<GeneroResponseDTO, Genero>().ReverseMap();
        }
        private void ConfigureMapperActores()
        {
            CreateMap<ActoresRequestDTO, Actor>().ForMember(x => x.Foto, opciones => opciones.Ignore());
            CreateMap<ActoresResponseDTO, Actor>().ReverseMap();
        }
    }

}
