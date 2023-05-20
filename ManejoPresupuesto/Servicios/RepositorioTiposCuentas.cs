using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
        Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados);
    }
    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;

        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        //Crear tipoCuenta pero de manera ordenada 
        public async Task Crear(TipoCuenta tipoCuenta)
        {

            /*
             Como lo tenemos en un procedimiento almacenado, le pasamos los 2 parametros que acepta
             creando un objeto anonimo con 2 propiedades a pasar.

             QuerySingleAsync devuelve o afecta solo a una fila 
             */

            using var connection= new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("TiposCuentasInsertar", new {nombre=tipoCuenta.Nombre,
                                                                                          usuarioId=tipoCuenta.UsuarioId},
                                                                                          commandType:System.Data.CommandType.StoredProcedure);
            tipoCuenta.Id = id;
        }

        //Validacion a nivel controlador
        public async Task<bool> Existe(string nombre, int usuarioId)
        {

            //QueryFirstOrDefault devuelve 1 si encuentra y 0 si no 
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas WHERE Nombre=@Nombre AND UsuarioId=@UsuarioId",
                                                                         new {nombre, usuarioId} );

            return existe == 1;
        }

        //
        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            //QueryAsync<TipoCuenta> mapea los datos en el tipo especificado, en este caso TipoCuenta
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden FROM TiposCuentas WHERE UsuarioId=@UsuarioId
                                                             ORDER BY Orden", new { usuarioId });

        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            //Execute hace una query cuando no devuelve nada, es decir, para modificar 
            using var connection = new SqlConnection( connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas SET Nombre=@Nombre WHERE Id=@Id", tipoCuenta );
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id,Nombre, Orden FROM TiposCuentas WHERE Id= @Id AND UsuarioId=@UsuarioId", 
                                                                          new {id, usuarioId} );
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE TiposCuentas WHERE Id=@Id", new {id});
        }

        public async Task Ordenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados)
        {
            var query = " UPDATE TiposCuentas SET Orden=@Orden WHERE Id=@Id;";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, tipoCuentasOrdenados);
        }

    }
}
