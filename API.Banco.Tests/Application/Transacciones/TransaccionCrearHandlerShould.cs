using API.Banco.Application.UsesCases.Transacciones;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Enums;
using API.Banco.Domain.Interfaces;
using Moq;

namespace API.Banco.Tests.Application.Transacciones
{
    public class TransaccionCrearHandlerShould
    {
        [Fact]
        public async Task HandleAsync_ShouldCrearDeposito_CuandoCuentaExiste()
        {
            // Arrange
            var mockCuentaRepo = new Mock<ICuentaRepository>();
            var mockTransaccionRepo = new Mock<ITransaccionRepository>();

            // La cuenta existe
            mockCuentaRepo.Setup(r => r.ExisteCuentaNumero("123456"))
                          .ReturnsAsync(1);
            mockCuentaRepo.Setup(r => r.ObtenerSaldoActual("123456"))
                          .ReturnsAsync(1000);
            mockTransaccionRepo.Setup(r => r.CrearTransaccion(It.IsAny<Transaccion>()))
                          .ReturnsAsync((Transaccion t) => t);
            mockCuentaRepo.Setup(r => r.ActualizarSaldo("123456", It.IsAny<decimal>()))
                          .Returns(Task.CompletedTask);

            var handler = new TransaccionCrearHandler(mockCuentaRepo.Object, mockTransaccionRepo.Object);

            // Act
            var result = await handler.HandleAsync("123456", 500, TipoTransaccion.Deposito, "usuario1");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Data.IdCuenta);
            Assert.Equal(1500, result.Data.SaldoDisponible);
        }

        [Fact]
        public async Task HandleAsync_Failed_RetiroSaldoInsuficiente()
        {
            // Arrange
            var mockCuentaRepo = new Mock<ICuentaRepository>();
            var mockTransaccionRepo = new Mock<ITransaccionRepository>();

            // La cuenta existe pero saldo insuficiente
            mockCuentaRepo.Setup(r => r.ExisteCuentaNumero("123456"))
                          .ReturnsAsync(1);
            mockCuentaRepo.Setup(r => r.ObtenerSaldoActual("123456"))
                          .ReturnsAsync(100); // saldo bajo

            var handler = new TransaccionCrearHandler(mockCuentaRepo.Object, mockTransaccionRepo.Object);

            // Act
            var result = await handler.HandleAsync("123456", 500, TipoTransaccion.Retiro, "usuario1");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Saldo insuficiente para realizar la transacción.", result.Message);
        }

        [Fact]
        public async Task HandleAsync_Failed_CuentaNoExiste()
        {
            // Arrange
            var mockCuentaRepo = new Mock<ICuentaRepository>();
            var mockTransaccionRepo = new Mock<ITransaccionRepository>();

            mockCuentaRepo.Setup(r => r.ExisteCuentaNumero("999999"))
                          .ReturnsAsync(0);

            var handler = new TransaccionCrearHandler(mockCuentaRepo.Object, mockTransaccionRepo.Object);

            // Act
            var result = await handler.HandleAsync("999999", 500, TipoTransaccion.Deposito, "usuario1");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La cuenta no existe.", result.Message);
        }
    }
}
