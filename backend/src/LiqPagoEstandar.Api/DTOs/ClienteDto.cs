using System.ComponentModel.DataAnnotations;

namespace LiqPagoEstandar.Api.DTOs;

public record ClienteDto(
    int Id,
    string Dni,
    string Cuit,
    string Sexo,
    string Apellido,
    string Nombre,
    DateOnly FechaNacimiento,
    string? Email,
    string? Telefono,
    string? Direccion,
    bool Activo
);

public class ClienteRequest
{
    [Required]
    [RegularExpression(@"^\d{1,8}$", ErrorMessage = "El DNI debe tener entre 1 y 8 dígitos numéricos.")]
    public string Dni { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[MF]$", ErrorMessage = "El sexo debe ser 'M' o 'F'.")]
    public string Sexo { get; set; } = string.Empty;

    [Required]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public DateOnly? FechaNacimiento { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}
