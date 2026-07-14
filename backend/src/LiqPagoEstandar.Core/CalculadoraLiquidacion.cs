namespace LiqPagoEstandar.Core;

public static class CalculadoraLiquidacion
{
    public static decimal SueldoBasicoNormal(Personal personal) =>
        personal.HorasMensualesPactadas * personal.ValorHoraBase;

    public static decimal TotalHorasNormales(Personal personal, Novedad novedad) =>
        novedad.HorasNormales * personal.ValorHoraBase;

    public static int AniosAntiguedad(Personal personal, DateOnly fechaActual)
    {
        var anios = fechaActual.Year - personal.FechaIngreso.Year;
        if (fechaActual < personal.FechaIngreso.AddYears(anios))
        {
            anios--;
        }
        return anios;
    }

    public static decimal ItemAntiguedad(decimal sueldoBasicoNormal, int aniosAntiguedad, ParametrosLiquidacion parametros) =>
        sueldoBasicoNormal * parametros.PorcentajeAntiguedad * aniosAntiguedad;

    public static decimal ItemZonaDesfavorable(
        decimal sueldoBasicoNormal,
        decimal itemAntiguedad,
        Personal personal,
        ZonaDesfavorable zonaDesfavorable,
        ParametrosLiquidacion parametros
    ) =>
        zonaDesfavorable.AplicaA(personal.Provincia)
            ? (sueldoBasicoNormal + itemAntiguedad) * parametros.PorcentajeZonaDesfavorable
            : 0m;

    public static decimal ItemHorasExtras(Personal personal, Novedad novedad, ParametrosLiquidacion parametros) =>
        personal.ValorHoraBase * parametros.MultiplicadorHorasExtras * novedad.HorasExtra;

    public static decimal ItemFeriados(Personal personal, Novedad novedad, ParametrosLiquidacion parametros) =>
        personal.ValorHoraBase * parametros.MultiplicadorFeriados * novedad.HorasFeriado;

    public static decimal TotalAPagar(
        decimal totalHorasNormales,
        decimal itemHorasExtras,
        decimal itemAntiguedad,
        decimal itemFeriados,
        decimal itemZonaDesfavorable
    ) =>
        totalHorasNormales + itemHorasExtras + itemAntiguedad + itemFeriados + itemZonaDesfavorable;
}
