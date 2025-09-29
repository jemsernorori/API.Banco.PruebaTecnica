using API.Banco.Application.DTOs;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Enums;
using API.Banco.Domain.Interfaces;

namespace API.Banco.Application.UsesCases.Transacciones
{
    public class TransaccionCrearHandler
    {
        private readonly ICuentaRepository _cuentaRepository;
        private readonly ITransaccionRepository _transaccionRepository;

        public TransaccionCrearHandler(ICuentaRepository cuentaRepo, ITransaccionRepository transaccionRepo)
        {
            _cuentaRepository = cuentaRepo;
            _transaccionRepository = transaccionRepo;
        }

        public async Task<Result<Transaccion>> HandleAsync(string numeroCuenta, decimal monto, TipoTransaccion tipo, string usuario, string? descripcion = null)
        {
            
            var idCuenta = await _cuentaRepository.ExisteCuentaNumero(numeroCuenta);
            if (idCuenta == 0)
                return Result<Transaccion>.Failure("La cuenta no existe.");


            var saldoActual = await _cuentaRepository.ObtenerSaldoActual(numeroCuenta);

            if ((tipo == TipoTransaccion.Retiro || tipo == TipoTransaccion.TransferenciaEnviada) && saldoActual < monto)
                return Result<Transaccion>.Failure("Saldo insuficiente para realizar la transacción.");

            decimal nuevoSaldo = tipo switch
            {
                TipoTransaccion.Deposito => saldoActual + monto,
                TipoTransaccion.Retiro => saldoActual - monto,
                TipoTransaccion.TransferenciaEnviada => saldoActual - monto,
                TipoTransaccion.TransferenciaRecibida => saldoActual + monto,
                _ => saldoActual
            };

            // Creamos la transaccion
            var transaccion = new Transaccion
            {
                IdCuenta = idCuenta,
                NumeroCuenta = numeroCuenta,
                TipoTransaccion = tipo,
                FechaMovimiento = DateTime.Now,
                Monto = monto,
                SaldoDisponible = nuevoSaldo,
                Descripcion = descripcion,
                FechaCreacion = DateTime.Now,
                UsuarioCreacion = usuario
            };

            await _transaccionRepository.CrearTransaccion(transaccion);

            // Actualizamos el saldo de la cuenta
            await _cuentaRepository.ActualizarSaldo(numeroCuenta, nuevoSaldo);

            return Result<Transaccion>.Success(transaccion);
        }
    }
}
