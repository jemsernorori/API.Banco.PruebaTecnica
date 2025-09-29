using API.Banco.Application.DTOs;
using API.Banco.Application.UsesCases.Cuentas;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;
using Moq;

namespace API.Banco.Tests.Application.Cuentas
{
    public class CuentaObtenerHandlerShould
    {
        [Fact]
        public async Task HandleAsync_ShouldObtenerCuenta_CuandoExiste()
        {
            // Arrange
            var mockCuentaRepo = new Mock<ICuentaRepository>();
            var cuentaExistente = new Cuenta
            {
                IdCuenta = 1,
                NumeroCuenta = "123456",
                SaldoActual = 5000,
                TipoCuenta = "Ahorros",
                Moneda = "USD",
                Estado = "Activo",
                NombreCliente = "Juan Perez",
                IdentificacionCliente = "12345678"
            };

            mockCuentaRepo.Setup(r => r.ObtenerCuenta(It.IsAny<Cuenta>()))
                          .ReturnsAsync(cuentaExistente);

            var handler = new CuentaObtenerHandler(mockCuentaRepo.Object);

            var request = new CuentaConsultarRequestDTO { NumeroCuenta = "123456" };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("123456", result.Data.NumeroCuenta);
            Assert.Equal(5000, result.Data.SaldoActual);
        }

        [Fact]
        public async Task HandleAsync_FailedCuandoCuentaNoExiste()
        {
            // Arrange
            var mockCuentaRepo = new Mock<ICuentaRepository>();
            mockCuentaRepo.Setup(r => r.ObtenerCuenta(It.IsAny<Cuenta>()))
                          .ReturnsAsync((Cuenta)null);

            var handler = new CuentaObtenerHandler(mockCuentaRepo.Object);
            var request = new CuentaConsultarRequestDTO { NumeroCuenta = "999999" };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Cuenta no encontrada", result.Message);
        }
    }
}
