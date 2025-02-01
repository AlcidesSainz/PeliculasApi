namespace PeliculasApi.DTOs.Response
{
    public class ActoresFiltrarResponseDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        internal PaginacionResponseDTO Paginacion
        {
            get
            {
                return new PaginacionResponseDTO { Pagina = Pagina, RecordsPorPagina = RecordsPorPagina };
            }
        }

        public string? Nombre { get; set; }
    }
}
