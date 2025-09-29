using API.Banco.Domain.Enums;

namespace API.Banco.Application.DTOs
{
    public class CrearClienteRequest
    {
        public string PrimerNombre { get; set; } = null!;
        public string? SegundoNombre { get; set; }
        public string PrimerApellido { get; set; } = null!;
        public string SegundoApellido { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Identificacion { get; set; } = null!;
        public TipoIdentificacion TipoIdentificacion { get; set; }
        public Sexo Sexo { get; set; }
        public string Telefono { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public double MontoIngresos { get; set; }
    }
}
