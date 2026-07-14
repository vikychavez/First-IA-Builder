namespace LiqPagoEstandar.Data.Entities;

public class ClienteEntity
{
    public int Id { get; set; }
    public string Dni { get; set; } = string.Empty;
    public string Cuit { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public DateOnly FechaNacimiento { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime? FechaBaja { get; set; }
}
