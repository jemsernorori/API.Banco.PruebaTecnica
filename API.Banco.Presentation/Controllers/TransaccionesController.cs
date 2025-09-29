using API.Banco.Application.DTOs;
using API.Banco.Application.UsesCases.Transacciones;
using Microsoft.AspNetCore.Mvc;

namespace API.Banco.Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        private readonly TransaccionCrearHandler _transaccionCrearHandler;
        private readonly TransaccionResumenHandler _transaccionResumenHandler;
        public TransaccionesController(TransaccionCrearHandler transaccionCrearHandler, TransaccionResumenHandler transaccionResumenHandler)
        {
            this._transaccionCrearHandler = transaccionCrearHandler;
            this._transaccionResumenHandler = transaccionResumenHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CrearTransaccion([FromBody] TransaccionCrearRequestDTO request)
        {
            try
            {
                var result = await _transaccionCrearHandler.HandleAsync(
                request.NumeroCuenta,
                request.Monto,
                request.TipoTransaccion,
                request.UsuarioCreacion,
                request.Descripcion);

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
        public async Task<IActionResult> ObtenerResumen([FromBody] TransaccionRequestDTO request)
        {
            try
            {
                var result = await _transaccionResumenHandler.HandleAsync(request.NumeroCuenta);
                if (result.IsSuccess)
                {
                    return Ok(new
                    {
                        Transacciones = result.Data.Transacciones,
                        SaldoFinal = result.Data.SaldoFinal
                    });
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
