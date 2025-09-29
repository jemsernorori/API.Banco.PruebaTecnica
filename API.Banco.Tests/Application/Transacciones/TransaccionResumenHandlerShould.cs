using API.Banco.Application.UsesCases.Transacciones;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Enums;
using API.Banco.Domain.Interfaces;
using Moq;

namespace API.Banco.Tests.Application.Transacciones
{
    public class TransaccionResumenHandlerShould
    {
        [Fact]
        public async Task HandleAsync_ShouldDevolverResumen_CuandoCuentaTieneTransacciones()
        {
            // Arrange
            var mockTransRepo = new Mock<ITransaccionRepository>();
            var mockCuentaRepo = new Mock<ICuentaRepository>();

            mockCuentaRepo.Setup(r => r.ExisteCuentaNumero("123456"))
                         .ReturnsAsync(1);

            var transacciones = new List<Transaccion>
            {
                new Transaccion { IdTransaccion = 1, TipoTransaccion = TipoTransaccion.Deposito, Monto = 1000, SaldoDisponible = 1000 },
                new Transaccion { IdTransaccion = 2, TipoTransaccion = TipoTransaccion.Retiro, Monto = 200, SaldoDisponible = 800 }
            };

            mockTransRepo.Setup(r => r.ObtenerTransaccionesPorCuenta(1))
                         .ReturnsAsync(transacciones);

            var handler = new TransaccionResumenHandler(mockTransRepo.Object, mockCuentaRepo.Object);

            // Act
            var result = await handler.HandleAsync("123456");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Transacciones.Count);
            Assert.Equal(800, result.Data.SaldoFinal);
        }

        [Fact]
        public async Task HandleAsync_Failed_CuentaNoExiste()
        {
            // Arrange
            var mockTransRepo = new Mock<ITransaccionRepository>();
            var mockCuentaRepo = new Mock<ICuentaRepository>();

            mockCuentaRepo.Setup(r => r.ExisteCuentaNumero("999999"))
                         .ReturnsAsync(0);

            var handler = new TransaccionResumenHandler(mockTransRepo.Object, mockCuentaRepo.Object);

            // Act
            var result = await handler.HandleAsync("999999");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La cuenta no existe", result.Message);
        }

        [Fact]
        public async Task HandleAsync_Failed_NoHayTransacciones()
        {
            // Arrange
            var mockTransRepo = new Mock<ITransaccionRepository>();
            var mockCuentaRepo = new Mock<ICuentaRepository>();

            mockCuentaRepo.Setup(r => r.ExisteCuentaNumero("123456"))
                         .ReturnsAsync(1);

            mockTransRepo.Setup(r => r.ObtenerTransaccionesPorCuenta(1))
                         .ReturnsAsync(new List<Transaccion>()); // lista vacía

            var handler = new TransaccionResumenHandler(mockTransRepo.Object, mockCuentaRepo.Object);

            // Act
            var result = await handler.HandleAsync("123456");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("No hay transacciones para esta cuenta.", result.Message);
        }
    }
}
