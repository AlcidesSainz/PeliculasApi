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
            ConfigurarMappeoPeliculas();
        }

        private void ConfigurarMappeoPeliculas()
        {
            CreateMap<PeliculaRequestDTO, Pelicula>()
                .ForMember(x => x.Poster, opciones => opciones.Ignore())

                .ForMember(x => x.PeliculaGenero, dto =>
                    dto.MapFrom(p => p.GenerosIds!.Select(id => new PeliculaGenero { GeneroId = id })))

                .ForMember(x => x.PeliculaCine, dto
                    => dto.MapFrom(p => p.CinesIds!.Select(id => new PeliculaCine { CineId = id })))

                .ForMember(p => p.PeliculaActor, dto =>
                dto.MapFrom(p => p.Actores!.Select(actor => new PeliculaActor { ActorId = actor.Id, Personaje = actor.Personaje })));

            CreateMap<Pelicula, PeliculaResponseDTO>();
            CreateMap<PeliculaRequestDTO, PeliculaResponseDTO>();
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

            CreateMap<Actor, PeliculaActorResponseDTO>();
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
