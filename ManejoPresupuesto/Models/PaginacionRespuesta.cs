namespace ManejoPresupuesto.Models
{
    public class PaginacionRespuesta
    {
        public int Pagina { get; set; } =  1;
        public int RecordsPorPagina { get; set; } = 10;
        public int CantidadTotalRecords { get; set; }

        /* si tengo 100 registros y quiero mostrar 5 por pagina 
         * paginas  = 100/5
         */
        public int CantidadTotalDePaginas => (int)Math.Ceiling((double)CantidadTotalRecords / RecordsPorPagina);
        public string BaseURL { get; set; }
    }

    public class PaginacionRespuesta<T> : PaginacionRespuesta
    {
        public IEnumerable<T> Elementos { get; set; }
    }
}
