namespace API.Banco.Domain.Entities
{
    public class Cuenta
    {
        public int IdCuenta { get; set; }
        public string NumeroCuenta { get; set; } = null!;   // Se asigna por medio de Trigger en BD
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = null!;
        public string IdentificacionCliente { get; set; } = null!;
        public int IdTipoCuenta { get; set; }
        public string TipoCuenta { get; set; } = null!;
        public int IdEstado { get; set; }
        public string Estado { get; set; } = null!;
        public int IdMoneda { get; set; }
        public string Moneda { get; set; } = null!;
        public decimal SaldoInicial { get; set; }
        public decimal SaldoActual { get; set; } = 0;  // valor por defecto 0
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
}
