namespace API.Banco.Application.DTOs
{
    public class TransaccionResumenDTO
    {
        public int IdTransaccion { get; set; }
        public string TipoTransaccion { get; set; } = null!;
        public decimal Monto { get; set; }
        public decimal SaldoDisponible { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }
}
