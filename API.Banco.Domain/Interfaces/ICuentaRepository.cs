using API.Banco.Domain.Entities;

namespace API.Banco.Domain.Interfaces
{
    public interface ICuentaRepository
    {
        Task<Cuenta> ObtenerCuenta(Cuenta cuenta);
        Task<Cuenta> CrearCuenta(Cuenta cuenta);
        Task<bool> ExisteCuenta(int idCliente, int idTipoCuenta, int idMoneda);
        Task<int> ExisteCuentaNumero(string numeroCuenta);
        Task<decimal> ObtenerSaldoActual(string numeroCuenta);
        Task ActualizarSaldo(string numeroCuenta, decimal nuevoSaldo);


    }
}
