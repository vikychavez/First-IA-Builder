using System.ComponentModel.DataAnnotations;

namespace LiqPagoEstandar.Api.DTOs;

public record CategoriaDto(
    int Id,
    string Nombre,
    decimal ValorHoraConRetiro,
    decimal ValorHoraSinRetiro,
    bool Activo
);

public class CategoriaRequest
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public decimal? ValorHoraConRetiro { get; set; }

    [Required]
    public decimal? ValorHoraSinRetiro { get; set; }
}
