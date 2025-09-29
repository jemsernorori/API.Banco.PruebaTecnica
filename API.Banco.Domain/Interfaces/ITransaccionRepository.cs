using API.Banco.Domain.Entities;

namespace API.Banco.Domain.Interfaces
{
    public interface ITransaccionRepository
    {
        Task<Transaccion> CrearTransaccion(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> ObtenerTransaccionesPorCuenta(int IdCuenta);
    }
}
