using API.Banco.Application.DTOs;
using API.Banco.Application.UsesCases.Cuentas;
using Microsoft.AspNetCore.Mvc;

namespace API.Banco.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly CuentaCrearHandler _cuentaCrearHandler;
        private readonly CuentaObtenerHandler _cuentaObtenerHandler;
        private readonly CuentaAplicarInteresHandler _cuentaAplicarInteresHandler;

        public CuentasController(CuentaCrearHandler cuentaCrearHandler, CuentaObtenerHandler cuentaObtenerHandler, CuentaAplicarInteresHandler cuentaAplicarInteresHandler)
        {
            this._cuentaCrearHandler = cuentaCrearHandler;
            this._cuentaObtenerHandler = cuentaObtenerHandler;
            _cuentaAplicarInteresHandler = cuentaAplicarInteresHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CrearCuenta([FromBody] CuentaCrearRequestDTO cuenta)
        {
            try
            {
                string usuario = User?.Identity?.Name ?? "system";
                var result = await _cuentaCrearHandler.HandleAsync(cuenta, usuario);
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
        public async Task<IActionResult> ObtenerCuenta([FromBody] CuentaConsultarRequestDTO cuenta)
        {
            try
            {
                var result = await _cuentaObtenerHandler.HandleAsync(cuenta);
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

        [HttpPost]
        public async Task<IActionResult> AplicarInteres([FromBody] CuentaInteresRequestDTO request)
        {
            try
            {
                string usuario = User?.Identity?.Name ?? "system";
                request.UsuarioCreacion = usuario;
                var result = await _cuentaAplicarInteresHandler.HandleAsync(request);
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
    }
}
