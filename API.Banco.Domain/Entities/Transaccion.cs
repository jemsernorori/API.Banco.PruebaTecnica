using API.Banco.Domain.Enums;

namespace API.Banco.Domain.Entities
{
    public class Transaccion
    {
        public int IdTransaccion { get; set; }
        public string NumeroTransaccion { get; set; } = null!; // Se asigna por medio de Trigger en BD


        public int IdCuenta { get; set; }
        public string NumeroCuenta { get; set; } = null!;

        public TipoTransaccion TipoTransaccion { get; set; }


        public DateTime FechaMovimiento { get; set; }
        public decimal Monto { get; set; }
        public decimal SaldoDisponible { get; set; }
        public string? Descripcion { get; set; }


        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
}
