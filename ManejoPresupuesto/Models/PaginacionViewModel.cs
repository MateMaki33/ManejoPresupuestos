namespace ManejoPresupuesto.Models
{
    public class PaginacionViewModel
    {
        private int recordsPorPagina { get; set; } = 10;
        public int Pagina = 1;
        private readonly int cantidadMaximaRecordsPorPagina = 50;
        public int RecordsPorPagina 
        { 
            get { return recordsPorPagina; }
            set { recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value; }
        }

        public int RecordsASaltar => recordsPorPagina * (Pagina-1);
    }
}
