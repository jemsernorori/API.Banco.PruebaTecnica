using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;
using Dapper;
using System.Data;

namespace API.Banco.Infrastructure.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly IConnectionManager _connectionManager;

        public CuentaRepository(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        #region Métodos de Validación / Consultas 

        public async Task<bool> ExisteCuenta(int idCliente, int idTipoCuenta, int idMoneda)
        {
            using IDbConnection connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            var existe = await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(1)
                  FROM Cuenta A
                  INNER JOIN Cliente_Cuenta B ON A.Id_Cuenta = B.Id_Cuenta
                  WHERE B.Id_Cliente = @IdCliente
                  AND A.Id_Tipo_Cuenta = @IdTipoCuenta
                  AND A.Id_Moneda = @IdMoneda",
                new { IdCliente = idCliente, IdTipoCuenta = idTipoCuenta, IdMoneda = idMoneda });

            return existe > 0;
        }

        public async Task<decimal> ObtenerSaldoActual(string numeroCuenta)
        {
            using var connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            var saldo = await connection.ExecuteScalarAsync<decimal>(
                "SELECT Saldo_Actual FROM Cuenta WHERE numero_cuenta = @NumeroCuenta",
                new { NumeroCuenta = numeroCuenta });

            return saldo;
        }

        public async Task ActualizarSaldo(string numeroCuenta, decimal nuevoSaldo)
        {
            using var connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            await connection.ExecuteAsync(
                "UPDATE Cuenta SET Saldo_Actual = @Saldo WHERE Numero_Cuenta = @NumeroCuenta",
                new { Saldo = nuevoSaldo, NumeroCuenta = numeroCuenta });
        }
        public async Task<int> ExisteCuentaNumero(string numeroCuenta)
        {
            using IDbConnection connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();
            int idcuenta = await connection.ExecuteScalarAsync<int>(
                "SELECT Id_Cuenta FROM Cuenta WHERE Numero_Cuenta = @NumeroCuenta",
                new { NumeroCuenta = numeroCuenta });
            return idcuenta;
        }
        #endregion

        #region CRUD
        public async Task<Cuenta> CrearCuenta(Cuenta cuenta)
        {
            using IDbConnection connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            // Crear la cuenta
            var sql = @"
                INSERT INTO Cuenta
                ( Id_Tipo_Cuenta, Saldo_Inicial, Saldo_Actual, Id_Estado, Id_Moneda, Fecha_Creacion, Usuario_Creacion)
                VALUES
                (@IdTipoCuenta, @SaldoInicial, @SaldoInicial, 1, @IdMoneda, @FechaCreacion, @UsuarioCreacion);
                SELECT last_insert_rowid();";

            var id = await connection.ExecuteScalarAsync<long>(sql, cuenta);
            cuenta.IdCuenta = (int)id;

            // Recuperar número de cuenta
            var numeroCuenta = await connection.ExecuteScalarAsync<string>(
                "SELECT Numero_Cuenta FROM Cuenta WHERE Id_Cuenta = @IdCuenta",
                 new { cuenta.IdCuenta });

            cuenta.NumeroCuenta = numeroCuenta ?? "";

            // Insertar relación en Cliente_Cuenta
            var clienteCuenta = @"
                 INSERT INTO Cliente_Cuenta
                 (Id_Cliente, Id_Cuenta, Fecha_Creacion, Usuario_Creacion)
                 VALUES
                 (@IdCliente, @IdCuenta, @FechaCreacion, @UsuarioCreacion);";

            await connection.ExecuteAsync(clienteCuenta, new
            {
                cuenta.IdCliente,
                cuenta.IdCuenta,
                FechaCreacion = cuenta.FechaCreacion,
                cuenta.UsuarioCreacion
            });

            return cuenta;
        }

        public async Task<Cuenta> ObtenerCuenta(Cuenta cuenta)
        {
            using IDbConnection connection = _connectionManager.GetConnection(ConnectionManager.BD_KEY);
            connection.Open();

            var sql = @"
                  SELECT
                      A.Id_Cuenta AS IdCuenta,
                      A.Numero_Cuenta AS NumeroCuenta,
                      C.Id_Cliente AS IdCliente,
                      C.Primer_Nombre || ' ' || IFNULL(C.Segundo_Nombre, '') || ' ' || 
                      C.Primer_Apellido || ' ' || IFNULL(C.Segundo_Apellido, '') AS NombreCliente,
                      C.Identificacion AS IdentificacionCliente,
                      D.descripcion AS TipoCuenta,
                      A.Saldo_Inicial AS SaldoInicial,
                      A.Saldo_Actual AS SaldoActual,
                      E.descripcion AS Estado,
                      F.codigo AS Moneda,
                      A.Fecha_Creacion AS FechaCreacion,
                      A.Usuario_Creacion AS UsuarioCreacion
                  FROM cuenta A
                  INNER JOIN cliente_cuenta B
                      ON A.id_cuenta = B.Id_Cuenta
                  INNER JOIN clientes C
                      ON B.Id_Cliente = C.Id_Cliente
                  INNER JOIN tipo_cuenta D
                      ON A.id_tipo_cuenta = D.id_tipo_cuenta
                  INNER JOIN estado E 
                      ON A.Id_Estado = E.Id_Estado
                  INNER JOIN moneda F
                      ON A.id_moneda = F.id_moneda
                  WHERE A.Numero_Cuenta = @NumeroCuenta";

            var cuentas = await connection.QuerySingleOrDefaultAsync<Cuenta>(sql, new { NumeroCuenta = cuenta.NumeroCuenta });
            return cuentas;
        }
        #endregion
    }
}
