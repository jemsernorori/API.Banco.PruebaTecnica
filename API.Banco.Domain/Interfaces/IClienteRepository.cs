using API.Banco.Domain.Entities;

namespace API.Banco.Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> ObtenerCliente(Cliente cliente);
        Task<Cliente> CrearCliente(Cliente cliente);
        Task<bool> ClienteExiste(int idCliente);
    }
}
