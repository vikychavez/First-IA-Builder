namespace LiqPagoEstandar.Core;

public record Novedad(
    string PersonalDni,
    int Anio,
    int Mes,
    decimal HorasNormales,
    decimal HorasFeriado,
    decimal HorasExtra
);
