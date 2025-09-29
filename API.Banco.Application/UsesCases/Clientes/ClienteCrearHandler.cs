using API.Banco.Application.DTOs;
using API.Banco.Domain.Entities;
using API.Banco.Domain.Interfaces;

namespace API.Banco.Application.UsesCases.Clientes
{
    public class ClienteCrearHandler
    {
        private readonly IClienteRepository _clienteRepository;



        public ClienteCrearHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Result<ClienteResponseDTO>> HandleAsync(CrearClienteRequest request, string usuario)
        {
            //validaciones
            if (string.IsNullOrEmpty(request.PrimerNombre)) throw new ArgumentException("El nombre es requerido");
            if (string.IsNullOrEmpty(request.PrimerApellido)) throw new ArgumentException("El apellido es requerido");
            if (string.IsNullOrEmpty(request.Identificacion)) throw new ArgumentException("La identificación es requerida");
            if (request.FechaNacimiento > DateTime.Now) throw new ArgumentException("Fecha de nacimiento inválida");

            var cliente = new Cliente
            {
                PrimerNombre = request.PrimerNombre,
                SegundoNombre = request.SegundoNombre,
                PrimerApellido = request.PrimerApellido,
                SegundoApellido = request.SegundoApellido,
                FechaNacimiento = request.FechaNacimiento,
                Identificacion = request.Identificacion,
                IdTipoIdentificacion = (int)request.TipoIdentificacion,
                IdSexo = (int)request.Sexo,
                Telefono = request.Telefono,
                CorreoElectronico = request.CorreoElectronico,
                MontoIngresos = request.MontoIngresos,
                FechaCreacion = DateTime.Now,
                UsuarioCreacion = usuario
            };
            var clienteCreado = await _clienteRepository.CrearCliente(cliente);
            if (clienteCreado == null)
            {
                return Result<ClienteResponseDTO>.Failure("Error al crear el cliente");
            }

            var clienteRespondeDTO = new ClienteResponseDTO
            {
                IdCliente = clienteCreado.IdCliente ?? 0,
                Nombre = clienteCreado.PrimerNombre,
                SegundoNombre = clienteCreado.SegundoNombre,
                Apellido = clienteCreado.PrimerApellido,
                SegundoApellido = clienteCreado.SegundoApellido,
                Identificacion = clienteCreado.Identificacion
            };
            return Result<ClienteResponseDTO>.Success(clienteRespondeDTO);
        }
    }
}
