using System.ComponentModel.DataAnnotations;

namespace LiqPagoEstandar.Api.DTOs;

public record ZonaDesfavorableDto(int Id, string Provincia, bool Activo);

public class ZonaDesfavorableRequest
{
    [Required]
    public string Provincia { get; set; } = string.Empty;
}
