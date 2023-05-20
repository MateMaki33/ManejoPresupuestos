using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "Debe ser un correo válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }
    }
}
