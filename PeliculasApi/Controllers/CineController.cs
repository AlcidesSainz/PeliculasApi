﻿using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;

namespace PeliculasApi.Controllers
{
    [Route("api/cines")]
    [ApiController]
    public class CineController : CustomBaseController
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private const string cacheTag = "cines";

        public CineController(ApplicationDbContext dbContext, IMapper mapper, IOutputCacheStore outputCacheStore) : base(dbContext, mapper, outputCacheStore, cacheTag)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }
        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<CineResponseDTO>> Get([FromQuery] PaginacionResponseDTO paginacionResponseDTO)
        {
            return await Get<Cines, CineResponseDTO>(paginacionResponseDTO, ordenarPor: c => c.Nombre);
        }

        [HttpGet("{id:int}", Name = "ObtenerCinePorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<CineResponseDTO>> Get(int id)
        {
            return await Get<Cines, CineResponseDTO>(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CineRequestDTO cineRequestDTO)
        {
            return await Post<CineRequestDTO, Cines, CineResponseDTO>(cineRequestDTO, "ObtenerCinePorId");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] CineRequestDTO cineRequestDTO)
        {
            return await Put<CineRequestDTO, Cines>(id, cineRequestDTO);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Cines>(id);
        }
    }
}
