using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Banco.Application.DTOs
{
    public class CuentaConsultarResponseDTO
    {
        public string NumeroCuenta { get; set; } = null!;
        public decimal SaldoActual { get; set; }
        public string TipoCuenta { get; set; } = null!;
        public string Moneda { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string NombreCliente { get; set; } = null!;
        public string IdentificacionCliente { get; set; } = null!;
    }
}
