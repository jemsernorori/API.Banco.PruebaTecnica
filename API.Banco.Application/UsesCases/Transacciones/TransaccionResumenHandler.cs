using API.Banco.Application.DTOs;
using API.Banco.Domain.Interfaces;

namespace API.Banco.Application.UsesCases.Transacciones
{
    public class TransaccionResumenHandler
    {
        private readonly ITransaccionRepository _transaccionRepository;
        private readonly ICuentaRepository _cuentaRepository;

        public TransaccionResumenHandler(ITransaccionRepository transaccionRepository, ICuentaRepository cuentaRepository)
        {
            _transaccionRepository = transaccionRepository;
            _cuentaRepository = cuentaRepository;
        }

        public async Task<Result<(List<TransaccionResumenDTO> Transacciones, decimal SaldoFinal)>> HandleAsync(string numeroCuenta)
        {
            var idCuenta = await _cuentaRepository.ExisteCuentaNumero(numeroCuenta);
            if (idCuenta == 0)
                return Result<(List<TransaccionResumenDTO>, decimal)>.Failure("La cuenta no existe");

            // Obtener transacciones de la cuenta
            var transacciones = await _transaccionRepository.ObtenerTransaccionesPorCuenta(idCuenta);

            if (transacciones == null || !transacciones.Any())
                return Result<(List<TransaccionResumenDTO>, decimal)>.Failure("No hay transacciones para esta cuenta.");

            var resumen = transacciones.Select(t => new TransaccionResumenDTO
            {
                IdTransaccion = t.IdTransaccion,
                TipoTransaccion = t.TipoTransaccion.ToString(),
                Monto = t.Monto,
                SaldoDisponible = t.SaldoDisponible,
                FechaMovimiento = t.FechaMovimiento
            }).ToList();


            var saldoFinal = resumen.Last().SaldoDisponible;

            return Result<(List<TransaccionResumenDTO>, decimal)>.Success((resumen, saldoFinal));
        }
    }
}
