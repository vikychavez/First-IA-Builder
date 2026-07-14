namespace LiqPagoEstandar.Core;

// RF-28: porcentajes y multiplicadores editables sin cambio de código.
public record ParametrosLiquidacion(
    decimal PorcentajeAntiguedad,
    decimal PorcentajeZonaDesfavorable,
    decimal MultiplicadorHorasExtras,
    decimal MultiplicadorFeriados
);
