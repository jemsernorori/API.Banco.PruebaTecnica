using API.Banco.Application.DTOs;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;

namespace API.Banco.Application.UsesCases.Cuentas
{
    public class CuentaObtenerHandler
    {
        private readonly ICuentaRepository _cuentaRepository;

        public CuentaObtenerHandler(ICuentaRepository cuentaRepository)
        {
            _cuentaRepository = cuentaRepository;
        }

        public async Task<Result<CuentaConsultarResponseDTO>> HandleAsync(CuentaConsultarRequestDTO request)
        {
            var cuenta = new Cuenta
            {
                NumeroCuenta = request.NumeroCuenta
            };
            var cuentaObtenida = await _cuentaRepository.ObtenerCuenta(cuenta);
            if (cuentaObtenida == null)
            {
                return Result<CuentaConsultarResponseDTO>.Failure("Cuenta no encontrada");
            }
            var cuentaResponseDTO = new CuentaConsultarResponseDTO
            {
                NumeroCuenta = cuentaObtenida.NumeroCuenta,
                SaldoActual = cuentaObtenida.SaldoActual,
                TipoCuenta = cuentaObtenida.TipoCuenta,
                Moneda = cuentaObtenida.Moneda,
                Estado = cuentaObtenida.Estado,
                NombreCliente = cuentaObtenida.NombreCliente,
                IdentificacionCliente = cuentaObtenida.IdentificacionCliente
            };
            return Result<CuentaConsultarResponseDTO>.Success(cuentaResponseDTO);
        }
    }
}
