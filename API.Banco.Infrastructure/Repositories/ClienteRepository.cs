using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data;

namespace API.Banco.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IConnectionManager _connectionManager;

        public ClienteRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        public async Task<Cliente> CrearCliente(Cliente cliente)
        {
            using IDbConnection connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            var sql = @"
                INSERT INTO Clientes
                (Primer_Nombre, Segundo_Nombre, Primer_Apellido, Segundo_Apellido,
                 Fecha_Nacimiento, Identificacion, Id_Tipo_Identificacion, Id_Sexo,
                 Telefono, Correo_Electronico, Monto_Ingresos, Activo,
                 Fecha_Creacion, Usuario_Creacion)
                VALUES
                (@PrimerNombre, @SegundoNombre, @PrimerApellido, @SegundoApellido,
                 @FechaNacimiento, @Identificacion, @IdTipoIdentificacion, @IdSexo,
                 @Telefono, @CorreoElectronico, @MontoIngresos, 1,
                 @FechaCreacion, @UsuarioCreacion);
                SELECT last_insert_rowid();";

            try
            {
                var id = await connection.ExecuteScalarAsync<long>(sql, cliente);
                cliente.IdCliente = (int)id;
                return cliente;
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // UNIQUE constraint fail
            {
                throw new Exception($"El cliente con identificación {cliente.Identificacion} ya existe.", ex);
            }
        }

        public async Task<Cliente> ObtenerCliente(Cliente cliente)
        {
            using IDbConnection connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            var sql = @"
                SELECT
                        id_cliente AS IdCliente,
                        primer_nombre AS PrimerNombre,
                        segundo_nombre AS SegundoNombre,
                        primer_apellido AS PrimerApellido,
                        segundo_apellido AS SegundoApellido,
                        fecha_nacimiento AS FechaNacimiento,
                        identificacion AS Identificacion,
                        id_tipo_identificacion AS IdTipoIdentificacion,
                        id_sexo AS IdSexo,
                        telefono AS Telefono,
                        correo_electronico AS CorreoElectronico,
                        monto_ingresos AS MontoIngresos,
                        activo AS Activo,
                        fecha_creacion AS FechaCreacion,
                        usuario_creacion AS UsuarioCreacion,
                        fecha_modificacion AS FechaModificacion,
                        usuario_modificacion AS UsuarioModificacion
                FROM clientes
                WHERE (@IdCliente IS NULL OR id_cliente = @IdCliente)
                AND (@Identificacion IS NULL OR identificacion = @Identificacion)";

            var result = await connection.QuerySingleOrDefaultAsync<Cliente>(sql, new { IdCliente = cliente.IdCliente, Identificacion = cliente.Identificacion });

            return result;
        }
        public async Task<bool> ClienteExiste(int idCliente)
        {
            using IDbConnection connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            var existe = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM Clientes WHERE Id_Cliente = @IdCliente AND Activo = 1",
                new { IdCliente = idCliente });

            return existe > 0;
        }
    }
}
