namespace API.Banco.Application.DTOs
{
    public class CuentaInteresResponseDTO
    {
        public string NumeroCuenta { get; set; } = null!;
        public decimal SaldoActual { get; set; }
        public decimal InteresGenerado { get; set; }
    }
}
