using Microsoft.AspNetCore.Identity;

//Clase para traducir los errores de Identity a Español y los pasamos en la clase program

namespace ManejoPresupuesto.Servicios
{
    public class MensajesDeErrorIdentity: IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return new IdentityError { Code = nameof(DefaultError), Description = $"Ha ocurrido un error." };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError { Code = nameof(PasswordMismatch), Description = $"la contraseña no coincide" };
        }
    }
}
