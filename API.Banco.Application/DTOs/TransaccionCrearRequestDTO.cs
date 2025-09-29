using API.Banco.Domain.Enums;

namespace API.Banco.Application.DTOs
{
    public class TransaccionCrearRequestDTO
    {
        public string NumeroCuenta { get; set; } = null!;
        public decimal Monto { get; set; }
        public TipoTransaccion TipoTransaccion { get; set; }
        public string? Descripcion { get; set; }
        public string UsuarioCreacion { get; set; } = null!;

    }
}
