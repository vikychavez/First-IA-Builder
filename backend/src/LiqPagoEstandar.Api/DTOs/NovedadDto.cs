using System.ComponentModel.DataAnnotations;

namespace LiqPagoEstandar.Api.DTOs;

public record NovedadDto(
    int PersonalId,
    string Dni,
    string ClienteNombre,
    string ApellidoNombre,
    decimal HorasNormales,
    decimal HorasFeriado,
    decimal HorasExtra
);

public class NovedadUpsertRequest
{
    [Required]
    public int PersonalId { get; set; }

    [Required]
    public int? Anio { get; set; }

    [Required]
    public int? Mes { get; set; }

    [Required]
    public decimal? HorasNormales { get; set; }

    [Required]
    public decimal? HorasFeriado { get; set; }

    [Required]
    public decimal? HorasExtra { get; set; }
}
