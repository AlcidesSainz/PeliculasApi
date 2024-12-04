using PeliculasApi.DTOs.Response;

namespace PeliculasApi.Utilities
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionResponseDTO paginacion)
        {
            return queryable.Skip((paginacion.Pagina - 1) * paginacion.RecordsPorPagina).Take(paginacion.RecordsPorPagina);
        }
    }
}
