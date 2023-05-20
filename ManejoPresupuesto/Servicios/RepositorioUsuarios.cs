using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioUsuarios
    {
        Task<Usuario> BuscarUsuarioPorMail(string emailNormalizado);
        Task<int> CrearUsuario(Usuario usuario);
    }
    public class RepositorioUsuarios: IRepositorioUsuarios
    {
        private readonly string connectionString;

        public RepositorioUsuarios(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

         public async Task<int> CrearUsuario(Usuario usuario)
        {
            using var connection = new SqlConnection(connectionString);
            var Usuarioid = await connection.QuerySingleAsync<int>(@"
                   INSERT INTO Usuarios (Email, EmailNormalizado, PasswordHash) 
                   VALUES (@Email, @EmailNormalizado, @PasswordHash); SELECT SCOPE_IDENTITY();", usuario);

            await connection.ExecuteAsync("CrearDatosUsuarioNuevo", new { Usuarioid }, commandType: System.Data.CommandType.StoredProcedure);
            return Usuarioid;
        }

        public async Task<Usuario> BuscarUsuarioPorMail(string emailNormalizado)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Usuario>(@"
                  SELECT * FROM Usuarios Where EmailNormalizado = @emailNormalizado", new { emailNormalizado });
        }

    }
}
