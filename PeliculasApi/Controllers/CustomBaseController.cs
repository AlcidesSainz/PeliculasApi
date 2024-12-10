using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using PeliculasApi.DTOs.Request;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Entidades;
using PeliculasApi.Utilities;
using System.Linq.Expressions;

namespace PeliculasApi.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly string cacheTag;

        public CustomBaseController(ApplicationDbContext dbContext, IMapper mapper, IOutputCacheStore outputCacheStore, string cacheTag)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.cacheTag = cacheTag;
        }
        protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionResponseDTO paginacionResponseDTO, Expression<Func<TEntidad, object>> ordenarPor) where TEntidad : class
        {
            var queryable = dbContext.Set<TEntidad>().AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.OrderBy(ordenarPor).Paginar(paginacionResponseDTO).ProjectTo<TDTO>(mapper.ConfigurationProvider).ToListAsync();
        }
        protected async Task<ActionResult<TDTO>> Get<TEntidad, TDTO>(int id)
            where TEntidad : class, IId
            where TDTO : IId
        {
            var entidad = await dbContext.Set<TEntidad>().ProjectTo<TDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            return entidad;
        }
        protected async Task<IActionResult> Post<TRequestDTO, TEntidad, TDTO>(TRequestDTO requestDTO, string nombreRuta)
            where TEntidad : class, IId
        {
            var entidad = mapper.Map<TEntidad>(requestDTO);
            dbContext.Add(entidad);
            await dbContext.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            var entidadDTO = mapper.Map<TDTO>(entidad);
            return CreatedAtRoute(nombreRuta, new { id = entidad.Id }, entidadDTO);
        }
        protected async Task<IActionResult> Put<TRequestDTO, TEntidad>(int id, TRequestDTO requestDTO)
            where TEntidad : class, IId
        {
            var entidadExiste = await dbContext.Set<TEntidad>().AnyAsync(g => g.Id == id);
            if (entidadExiste == false)
            {
                return NotFound();
            }
            var entidad = mapper.Map<TEntidad>(requestDTO);
            entidad.Id = id;

            dbContext.Update(entidad);
            await dbContext.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        protected async Task<IActionResult> Delete<TEntidad>(int id)
        where TEntidad : class, IId
        {
            var registrosBorrados = await dbContext.Set<TEntidad>().Where(g => g.Id == id).ExecuteDeleteAsync();
            if (registrosBorrados == 0)
            {
                return NotFound();
            }
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }
    }

}
