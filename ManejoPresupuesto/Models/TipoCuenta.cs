using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta /*: IValidatableObject */
    {
        public int Id { get; set; }

        [Remote(action: "VerificarExisteTipoCuenta", controller: "TiposCuentas")] //recibe el Json enviado desde el controlador con httpget
        [PrimeraLetraMayuscula] //validacion propia en carpeta Validaciones
        [Required(ErrorMessage ="El campo {0} es obligatorio")] //atributos para validar pasando un mensaje {0}=esta propiedad
        public string Nombre { get; set; }

        public int UsuarioId { get; set; }

        public int Orden { get; set; }


        //VALIDACION POR MODELO 

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if( Nombre != null && Nombre.Length >0 ) { 
        //        var primeraLetra = Nombre[0].ToString();

        //        if(primeraLetra != primeraLetra.ToUpper())
        //        {
        //           yield return new ValidationResult ("La primera letra debe ser mayuscula", new[] {nameof(Nombre)});
        //        }
        //    }
        //}

        /* 
         OTRAS VALIDACIONES POR DEFECTO CON ATRIBUTOS
         
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [EmailAddress(ErrorMessage ="Debe ser un correo electronico válido")]
        public string Email { get; set; }

        [Range(minimum:18, maximum:130, ErrorMessage ="El valor del campo {0} debe ser entre {1} y {2}")]
        public int Edad { get; set; }

        [Url(ErrorMessage ="El campo {0} debe ser una url válida")]
        public string URL { get; set; }

        [CreditCard(ErrorMessage ="La terjeta de crédito no es válida")]
        public string TarjetaDeCredito { get; set; }

        */
    }
}
