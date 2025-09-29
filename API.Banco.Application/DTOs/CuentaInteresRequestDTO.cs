namespace API.Banco.Application.DTOs
{
    public class CuentaInteresRequestDTO
    {
        public string NumeroCuenta { get; set; } = null!;
        public decimal TasaInteres { get; set; }
        public string UsuarioCreacion { get; set; } = null!;
    }
}
