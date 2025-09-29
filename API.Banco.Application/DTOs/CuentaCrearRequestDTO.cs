namespace API.Banco.Application.DTOs
{
    public class CuentaCrearRequestDTO
    {
        public int IdCliente { get; set; }
        public int IdTipoCuenta { get; set; }
        public int IdMoneda { get; set; }
        public decimal SaldoInicial { get; set; }
        public string UsuarioCreacion { get; set; } = null!;
    }
}
