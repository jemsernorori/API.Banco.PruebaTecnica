using API.Banco.Application.DTOs;
using API.Banco.Application.UsesCases.Clientes;
using Microsoft.AspNetCore.Mvc;

namespace API.Banco.Presentation.Controllers
{

    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        private readonly ClienteCrearHandler _clienteCrearHandler;
        private readonly ClienteObtenerHandler _clienteObtenerHandler;

        public ClientesController(ClienteCrearHandler clienteCrearHandler, ClienteObtenerHandler clienteObtenerHandler)
        {
            this._clienteCrearHandler = clienteCrearHandler;
            this._clienteObtenerHandler = clienteObtenerHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteRequest cliente)
        {
            try
            {
                string usuario = User?.Identity?.Name ?? "system";
                var result = await _clienteCrearHandler.HandleAsync(cliente, usuario);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                else
                {
                    return BadRequest(new { message = result.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error interno: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerCliente([FromBody] ClienteObtenerRequest cliente)
        {
            try
            {
                var result = await _clienteObtenerHandler.HandleAsync(cliente);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                else
                {
                    return NotFound(new { message = result.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error interno: {ex.Message}" });
            }
        }
    }
}
