using API.Banco.Application.DTOs;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;

namespace API.Banco.Application.UsesCases.Clientes
{
    public class ClienteObtenerHandler
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteObtenerHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }



        public async Task<Result<ClienteDetalleDTO>> HandleAsync(ClienteObtenerRequest request)
        {
            var cliente = new Cliente
            {
                IdCliente = request.IdCliente,
                Identificacion = request.Identificacion ?? null
            };

            var clienteObtener = await _clienteRepository.ObtenerCliente(cliente);

            if (clienteObtener == null)
            {
                return Result<ClienteDetalleDTO>.Failure("Cliente No encontrado");
            }

            var clienteDetalleDTO = new ClienteDetalleDTO
            {
                IdCliente = clienteObtener.IdCliente ?? 0,
                PrimerNombre = clienteObtener.PrimerNombre,
                SegundoNombre = clienteObtener.SegundoNombre ?? "",
                PrimerApellido = clienteObtener.PrimerApellido,
                SegundoApellido = clienteObtener.SegundoApellido ?? "",
                Identificacion = clienteObtener.Identificacion,
                Telefono = clienteObtener.Telefono,
                CorreoElectronico = clienteObtener.CorreoElectronico,
                FechaNacimiento = clienteObtener.FechaNacimiento,
                Activo = clienteObtener.Activo,
                MontoIngresos = (decimal)clienteObtener.MontoIngresos,
                IdSexo = clienteObtener.IdSexo,
                IdTipoIdentificacion = clienteObtener.IdTipoIdentificacion,
                FechaCreacion = clienteObtener.FechaCreacion,
                UsuarioCreacion = clienteObtener.UsuarioCreacion,
                FechaModificacion = clienteObtener.FechaModificacion,
                UsuarioModificacion = clienteObtener.UsuarioModificacion

            };
            return Result<ClienteDetalleDTO>.Success(clienteDetalleDTO);
        }
    }
}
