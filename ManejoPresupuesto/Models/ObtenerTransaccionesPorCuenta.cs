/*Esta clase se utiliza para realizar una consulta sql con sus propiedades como parametros */

namespace ManejoPresupuesto.Models
{
    public class ObtenerTransaccionesPorCuenta
    {
        public int UsuarioId { get; set; }
        public int CuentaId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
