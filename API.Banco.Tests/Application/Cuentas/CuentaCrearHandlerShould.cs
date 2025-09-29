using API.Banco.Application.DTOs;
using API.Banco.Application.UsesCases.Cuentas;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;
using Moq;

namespace API.Banco.Tests.Application.Cuentas
{
    public class CuentaCrearHandlerShould
    {
        [Fact]
        public async Task HandleAsync_ShouldCrearCuenta_CuandoClienteExiste()
        {
            // Arrange
            var mockCuentaRepo = new Mock<ICuentaRepository>();
            var mockClienteRepo = new Mock<IClienteRepository>();

            mockClienteRepo.Setup(r => r.ClienteExiste(It.IsAny<int>()))
                           .ReturnsAsync(true);

            mockCuentaRepo.Setup(r => r.ExisteCuenta(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                          .ReturnsAsync(false);

            mockCuentaRepo.Setup(r => r.CrearCuenta(It.IsAny<Cuenta>()))
                          .ReturnsAsync((Cuenta c) => { c.IdCuenta = 1; c.NumeroCuenta = "123456"; return c; });

            var handler = new CuentaCrearHandler(mockCuentaRepo.Object, mockClienteRepo.Object);

            var request = new CuentaCrearRequestDTO
            {
                IdCliente = 1,
                IdTipoCuenta = 1,
                IdMoneda = 1,
                SaldoInicial = 1000
            };

            // Act
            var result = await handler.HandleAsync(request, "usuario1");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Data.IdCuenta);
            Assert.Equal("123456", result.Data.NumeroCuenta);
        }

        [Fact]
        public async Task HandleAsync_FailedCuandoClienteNoExiste()
        {
            // Arrange
            var mockCuentaRepo = new Mock<ICuentaRepository>();
            var mockClienteRepo = new Mock<IClienteRepository>();

            mockClienteRepo.Setup(r => r.ClienteExiste(It.IsAny<int>()))
                           .ReturnsAsync(false);

            var handler = new CuentaCrearHandler(mockCuentaRepo.Object, mockClienteRepo.Object);

            var request = new CuentaCrearRequestDTO
            {
                IdCliente = 99,
                IdTipoCuenta = 1,
                IdMoneda = 1,
                SaldoInicial = 1000
            };

            // Act
            var result = await handler.HandleAsync(request, "usuario1");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("El cliente no existe o no está activo.", result.Message);
        }
    }
}
