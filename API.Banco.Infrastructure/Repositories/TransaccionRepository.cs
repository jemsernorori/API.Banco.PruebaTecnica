using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;
using Dapper;

namespace API.Banco.Infrastructure.Repositories
{
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly IConnectionManager _connectionManager;
        public TransaccionRepository(IConnectionManager connectionManager) => _connectionManager = connectionManager;

        public async Task<Transaccion> CrearTransaccion(Transaccion transaccion)
        {
            using var connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            var sql = @"
                INSERT INTO Transaccion_Cuenta
                (Id_Cuenta, Id_Tipo_Transaccion, Fecha_Movimiento, Monto, Saldo_Disponible, Descripcion, Fecha_Creacion, Usuario_Creacion)
                VALUES (@IdCuenta, @TipoTransaccion, @FechaMovimiento, @Monto, @SaldoDisponible, @Descripcion, @FechaCreacion, @UsuarioCreacion);
                SELECT last_insert_rowid();";

            var id = await connection.ExecuteScalarAsync<long>(sql, transaccion);
            transaccion.IdTransaccion = (int)id;


            var numero = await connection.ExecuteScalarAsync<string>(
                "SELECT Numero_Transaccion FROM Transaccion_Cuenta WHERE Id_Transaccion = @Id",
                new { Id = id });
            transaccion.NumeroTransaccion = numero ?? "";

            return transaccion;
        }

        public async Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorCuenta(int idCuenta)
        {
            using var connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            var sql = @"
                 SELECT Id_Transaccion AS IdTransaccion,
                        Id_Cuenta AS IdCuenta,
                        Id_Tipo_Transaccion AS TipoTransaccion,
                        Fecha_Movimiento AS FechaMovimiento,
                        Monto,
                        Saldo_Disponible AS SaldoDisponible,
                        Descripcion,
                        Fecha_Creacion AS FechaCreacion,
                        Usuario_Creacion AS UsuarioCreacion
                 FROM Transaccion_Cuenta
                 WHERE Id_Cuenta = @IdCuenta
                 ORDER BY Fecha_Movimiento ASC;";

            var result = await connection.QueryAsync<Transaccion>(sql, new { IdCuenta = idCuenta });
            return result;
        }
    }
}
