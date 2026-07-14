using System.ComponentModel.DataAnnotations;

namespace LiqPagoEstandar.Api.DTOs;

public record ParametrosLiquidacionDto(
    decimal PorcentajeAntiguedad,
    decimal PorcentajeZonaDesfavorable,
    decimal MultiplicadorHorasExtras,
    decimal MultiplicadorFeriados
);

public class ParametrosLiquidacionRequest
{
    [Required]
    public decimal? PorcentajeAntiguedad { get; set; }

    [Required]
    public decimal? PorcentajeZonaDesfavorable { get; set; }

    [Required]
    public decimal? MultiplicadorHorasExtras { get; set; }

    [Required]
    public decimal? MultiplicadorFeriados { get; set; }
}
