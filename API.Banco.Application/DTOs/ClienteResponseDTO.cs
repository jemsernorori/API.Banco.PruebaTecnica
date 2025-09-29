namespace API.Banco.Application.DTOs
{
    public class ClienteResponseDTO
    {
        public int IdCliente { get; set; }
        public string? Nombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? Apellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Identificacion { get; set; }
    }
}
