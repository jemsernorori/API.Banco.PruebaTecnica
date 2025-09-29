using API.Banco.Application.DTOs;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;

namespace API.Banco.Application.UsesCases.Cuentas
{
    public class CuentaCrearHandler
    {
        private readonly ICuentaRepository _cuentaRepository;
        private readonly IClienteRepository _clienteRepository;

        public CuentaCrearHandler(ICuentaRepository cuentaRepository, IClienteRepository clienteRepository)
        {
            _cuentaRepository = cuentaRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<Result<CuentaCrearResponseDTO>> HandleAsync(CuentaCrearRequestDTO request, string usuario)
        {
            // Validar que el cliente existe
            var clienteExiste = await _clienteRepository.ClienteExiste(request.IdCliente);
            if (!clienteExiste)
            {
                return Result<CuentaCrearResponseDTO>.Failure("El cliente no existe o no está activo.");
            }

            // Validar que no exista ya una cuenta con los mismos parámetros
            var cuentaDuplicada = await _cuentaRepository.ExisteCuenta(request.IdCliente, request.IdTipoCuenta, request.IdMoneda);
            if (cuentaDuplicada)
            {
                return Result<CuentaCrearResponseDTO>.Failure("El cliente ya tiene una cuenta activa con el mismo tipo y moneda.");
            }

            // Crear la cuenta
            var cuenta = new Cuenta
            {
                IdCliente = request.IdCliente,
                IdTipoCuenta = request.IdTipoCuenta,
                IdMoneda = request.IdMoneda,
                SaldoInicial = request.SaldoInicial,
                FechaCreacion = DateTime.Now,
                UsuarioCreacion = usuario
            };

            var cuentaCreada = await _cuentaRepository.CrearCuenta(cuenta);

            if (cuentaCreada == null)
            {
                return Result<CuentaCrearResponseDTO>.Failure("Error al crear la cuenta");
            }

            
            var cuentaResponseDTO = new CuentaCrearResponseDTO
            {
                NumeroCuenta = cuentaCreada.NumeroCuenta,
                IdCuenta = cuentaCreada.IdCuenta
            };

            return Result<CuentaCrearResponseDTO>.Success(cuentaResponseDTO);
        }
    }
}
