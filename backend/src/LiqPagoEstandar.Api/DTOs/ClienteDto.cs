using System.ComponentModel.DataAnnotations;

namespace LiqPagoEstandar.Api.DTOs;

public record ClienteDto(
    int Id,
    string Nombre,
    string? Email,
    string? Telefono,
    string? Direccion,
    bool Activo
);

public class ClienteRequest
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}
