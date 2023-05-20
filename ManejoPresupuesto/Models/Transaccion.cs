using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        [Display(Name = "Fecha Transacción")]
        [DataType(DataType.Date)]
        //DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd hh:MM tt")); para configurar como quieras
        public DateTime FechaTransaccion { get; set; } = DateTime.Today;

        public decimal Monto { get; set; }

        [Range(1, maximum:int.MaxValue, ErrorMessage="Debe seleccionar una categoría")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [StringLength(maximumLength:1000, ErrorMessage ="La nota no puede pasar de {1} caracteres")]
        public string Nota { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta")]
        [Display(Name = "Cuenta")]
        public int CuentaId { get; set; }


        [Display(Name = "Tipo de Operacion")]
        public TipoOperacion TipoOperacionId { get; set; } = TipoOperacion.Ingreso;

        /* Estas propiedades se añaden para mapear los aleas de un query en RepositorioTransacciones */
        public string Cuenta { get; set; }
        public string Categoria { get; set; }

    }
}
