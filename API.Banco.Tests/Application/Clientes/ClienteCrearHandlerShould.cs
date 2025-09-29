using API.Banco.Application.DTOs;
using API.Banco.Application.UsesCases.Clientes;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;
using Moq;

namespace API.Banco.Tests.Application.Clientes
{
    public class ClienteCrearHandlerShould
    {
        [Fact]
        public async Task CrearCliente_ShouldExito()
        {
            // Mock del repositorio
            var mockRepo = new Mock<IClienteRepository>();
            mockRepo.Setup(r => r.CrearCliente(It.IsAny<Cliente>()))
                    .ReturnsAsync((Cliente c) => { c.IdCliente = 1; return c; });

            var handler = new ClienteCrearHandler(mockRepo.Object);

            var request = new CrearClienteRequest
            {
                PrimerNombre = "Juan",
                PrimerApellido = "Perez",
                Identificacion = "12345678",
                FechaNacimiento = new DateTime(1990, 1, 1)
            };

            var result = await handler.HandleAsync(request, "usuario1");

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Data.IdCliente);
        }
    }
}
