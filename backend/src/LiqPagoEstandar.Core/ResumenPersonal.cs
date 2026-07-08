namespace LiqPagoEstandar.Core;

public record ResumenPersonal(
    Personal Personal,
    int Anio,
    int Mes,
    decimal SueldoBasicoNormal,
    decimal TotalHorasNormales,
    decimal ItemHorasExtras,
    int AniosAntiguedad,
    decimal ItemAntiguedad,
    decimal ItemFeriados,
    decimal ItemZonaDesfavorable,
    decimal TotalAPagar
);
