using AutoMapper;
using NetTopologySuite.Geometries;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;

namespace PeliculasApi.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            ConfigureMapperGeneros();
            ConfigureMapperActores();
            ConfigureMapperCines(geometryFactory);
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
        private void ConfigureMapperCines(GeometryFactory geometryFactory)
        {
            CreateMap<Cines, CineResponseDTO>()
                .ForMember(x => x.Latitud, cine => cine.MapFrom(p => p.Ubicacion.Y))
                .ForMember(x => x.Longitud, cine => cine.MapFrom(p => p.Ubicacion.X));
            CreateMap<CineRequestDTO, Cines>()
                .ForMember(x => x.Ubicacion, cineResponseDTO => cineResponseDTO.MapFrom(p => geometryFactory.CreatePoint(new Coordinate(p.Longitud, p.Latitud))));
        }
    }

}
