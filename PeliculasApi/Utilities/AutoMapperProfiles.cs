﻿using AutoMapper;
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

            CreateMap<Pelicula, PeliculasDetallesResponseDTO>()
                .ForMember(p => p.Generos, entidad => entidad.MapFrom(p => p.PeliculaGenero))
                .ForMember(p => p.Cines, entidad => entidad.MapFrom(p => p.PeliculaCine))
                .ForMember(p => p.Actores, entidad => entidad.MapFrom(p => p.PeliculaActor.OrderBy(a=>a.Orden)));

            CreateMap<PeliculaGenero, GeneroResponseDTO>()
                .ForMember(g => g.Id, pg => pg.MapFrom(p => p.GeneroId))
                .ForMember(g => g.Nombre, pg => pg.MapFrom(p => p.Genero.Nombre));

            CreateMap<PeliculaCine, CineResponseDTO>()
                .ForMember(c => c.Id, pg => pg.MapFrom(c => c.CineId))
                .ForMember(c => c.Nombre, pg => pg.MapFrom(c => c.Cine.Nombre))
                .ForMember(c => c.Latitud, pg => pg.MapFrom(c => c.Cine.Ubicacion.Y))
                .ForMember(c => c.Longitud, pg => pg.MapFrom(c => c.Cine.Ubicacion.X));

            CreateMap<PeliculaActor, PeliculaActorResponseDTO>()
                .ForMember(a => a.Id, entidad => entidad.MapFrom(p => p.ActorId))
                .ForMember(a => a.Nombre, entidad => entidad.MapFrom(p => p.Actor.Nombre))
                .ForMember(a => a.Foto, entidad => entidad.MapFrom(p => p.Actor.Foto))
                ;
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
            CreateMap<Cine, CineResponseDTO>()
                .ForMember(x => x.Latitud, cine => cine.MapFrom(p => p.Ubicacion.Y))
                .ForMember(x => x.Longitud, cine => cine.MapFrom(p => p.Ubicacion.X));
            CreateMap<CineRequestDTO, Cine>()
                .ForMember(x => x.Ubicacion, cineResponseDTO => cineResponseDTO.MapFrom(p => geometryFactory.CreatePoint(new Coordinate(p.Longitud, p.Latitud))));
        }
    }

}
