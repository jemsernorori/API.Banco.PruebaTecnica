namespace API.Banco.Domain.Entities
{
    public class Cliente
    {
        public int? IdCliente { get; set; }
        public string PrimerNombre { get; set; } = null!;
        public string? SegundoNombre { get; set; }
        public string PrimerApellido { get; set; } = null!;
        public string SegundoApellido { get; set; } = null!;
        public DateTime FechaNacimiento { get; set; }
        public string Identificacion { get; set; } = null!;
        public int IdTipoIdentificacion { get; set; }
        public int IdSexo { get; set; }
        public string Telefono { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public double MontoIngresos { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = null!;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
}
