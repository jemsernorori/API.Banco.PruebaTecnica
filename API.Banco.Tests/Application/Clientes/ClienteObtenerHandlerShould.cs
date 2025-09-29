using API.Banco.Application.DTOs;
using API.Banco.Application.UsesCases.Clientes;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;
using Moq;

namespace API.Banco.Tests.Application.Clientes
{
    public class ClienteObtenerHandlerShould
    {
        [Fact]
        public async Task HandleAsync_ShouldCliente_CuandoExiste()
        {
            // Arrange
            var mockRepo = new Mock<IClienteRepository>();
            var clienteExistente = new Cliente
            {
                IdCliente = 1,
                PrimerNombre = "Juan",
                SegundoNombre = "Carlos",
                PrimerApellido = "Perez",
                SegundoApellido = "Lopez",
                Identificacion = "12345678",
                Telefono = "555-1234",
                CorreoElectronico = "juan@example.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
                Activo = true,
                MontoIngresos = 1000,
                IdSexo = 1,
                IdTipoIdentificacion = 1,
                FechaCreacion = DateTime.Now,
                UsuarioCreacion = "usuario1"
            };

            mockRepo.Setup(r => r.ObtenerCliente(It.IsAny<Cliente>()))
                    .ReturnsAsync(clienteExistente);

            var handler = new ClienteObtenerHandler(mockRepo.Object);

            var request = new ClienteObtenerRequest { IdCliente = 1 };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Data.IdCliente);
            Assert.Equal("Juan", result.Data.PrimerNombre);
            Assert.Equal("Perez", result.Data.PrimerApellido);
        }

        [Fact]
        public async Task HandleAsync_Failed_CuandoNoExisteCliente()
        {
            // Arrange
            var mockRepo = new Mock<IClienteRepository>();
            mockRepo.Setup(r => r.ObtenerCliente(It.IsAny<Cliente>()))
                    .ReturnsAsync((Cliente)null);

            var handler = new ClienteObtenerHandler(mockRepo.Object);

            var request = new ClienteObtenerRequest { IdCliente = 99 };

            // Act
            var result = await handler.HandleAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Cliente No encontrado", result.Message);
        }
    }
}
