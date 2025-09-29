using API.Banco.Application.DTOs;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Enums;
using API.Banco.Domain.Interfaces;

namespace API.Banco.Application.UsesCases.Cuentas
{
    public class CuentaAplicarInteresHandler
    {
        private readonly ICuentaRepository _cuentaRepository;
        private readonly ITransaccionRepository _transaccionRepository;

        public CuentaAplicarInteresHandler(ICuentaRepository cuentaRepository, ITransaccionRepository transaccionRepository)
        {
            _cuentaRepository = cuentaRepository;
            _transaccionRepository = transaccionRepository;
        }

        public async Task<Result<CuentaInteresResponseDTO>> HandleAsync(CuentaInteresRequestDTO request)
        {
            var cuenta = new Cuenta
            {
                NumeroCuenta = request.NumeroCuenta
            };
            var cuentaObtenida = await _cuentaRepository.ObtenerCuenta(cuenta);
            if (cuentaObtenida == null)
            {
                return Result<CuentaInteresResponseDTO>.Failure("Cuenta no encontrada");
            }

            var saldoActual = await _cuentaRepository.ObtenerSaldoActual(cuenta.NumeroCuenta);

            if (saldoActual == 0)
                return Result<CuentaInteresResponseDTO>.Failure("El saldo actual es cero, no se pueden aplicar intereses.");

            var interes = saldoActual * request.TasaInteres / 100;
            var nuevoSaldo = saldoActual + interes;

            var transaccion = new Transaccion
            {
                IdCuenta = cuentaObtenida.IdCuenta,
                TipoTransaccion = TipoTransaccion.Intereses,
                FechaMovimiento = DateTime.Now,
                Monto = interes,
                SaldoDisponible = nuevoSaldo,
                Descripcion = $"Aplicación de interés del {request.TasaInteres}%",
                FechaCreacion = DateTime.Now,
                UsuarioCreacion = request.UsuarioCreacion
            };

            await _transaccionRepository.CrearTransaccion(transaccion);

            await _cuentaRepository.ActualizarSaldo(request.NumeroCuenta, nuevoSaldo);

            var response = new CuentaInteresResponseDTO
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                SaldoActual = saldoActual,
                InteresGenerado = interes
            };
            return Result<CuentaInteresResponseDTO>.Success(response);
        }
    }
}
