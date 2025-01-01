namespace PeliculasApi.DTOs.Response
{
    public class PeliculasFiltrarResponseDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }
        internal  PaginacionResponseDTO Paginacion
        {
            get
            {
                return new PaginacionResponseDTO { Pagina = Pagina, RecordsPorPagina = RecordsPorPagina };
            }
        }

        public string? Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool EnCines { get; set; }
        public bool ProximoEstreno { get; set; }
    }
}
