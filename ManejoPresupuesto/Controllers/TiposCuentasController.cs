using ManejoPresupuesto.Models;
using ManejoPresupuesto.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace ManejoPresupuesto.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServicioUsuarios servicioUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas, IServicioUsuarios servicioUsuarios)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.servicioUsuarios = servicioUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return View(tiposCuentas);
        }
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            //Validamos si el modelo es válido establecido con [required]. Si la propiedad 
            //correspondiente está vacía, no será válido y devolvemos los datos que ya se hayan insertado con un tag helper
            //para que no tenga que rellenar todo de nuevo

            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            tipoCuenta.UsuarioId = servicioUsuarios.ObtenerUsuarioId();

            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            if (yaExisteTipoCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }
            await repositorioTiposCuentas.Crear(tipoCuenta);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }

        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuentaExiste = await repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);

            if (tipoCuentaExiste is null)
            {
                return RedirectToAction("NoEncontrado", "Home");

            }
            await repositorioTiposCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var yaExiste = await repositorioTiposCuentas.Existe(nombre, usuarioId);

            if (yaExiste)
            {
                return Json($"El nombre {nombre} ya existe");
            }
            return Json(true);
        }

        //El metodo Borrar() es GET, nos manda a la vista Borrar, donde se ejecuta el metodo BorrarTipoCuenta() POST
        //De esta manera, si al darle al boton para ir a la vista el registro no existe redireccionamos
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuenta);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositorioTiposCuentas.Borrar(id);
            return RedirectToAction("Index");

        }


        /* 
           En la siguiente funcion se recibe por POST desde el body, un arreglo
           con los id segun el orden en el que están. Esta nos permite actualizar el orden en la base de datos
           con AJAX para que la ordenación se mantenga siempre.
         */

        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids) //recibe del cuerpo de la peticion http 
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            var idsTiposCuentas = tiposCuentas.Select(x => x.Id);

            var noPertenecenAlUsuario =ids.Except(idsTiposCuentas).ToList(); //Except compara los ids pasados del front con los ids
                                                                             //de la base de datos (idsTiposCuentas)

            if (noPertenecenAlUsuario.Count > 0)
            {
                return Forbid();
            }
            var tiposCuentasOrdenados = ids.Select((valor, indice)=> 
            new TipoCuenta{Id=valor,Orden=indice +1 }).AsEnumerable();
            await repositorioTiposCuentas.Ordenar(tiposCuentasOrdenados);
            return Ok();
        }
    }
}
