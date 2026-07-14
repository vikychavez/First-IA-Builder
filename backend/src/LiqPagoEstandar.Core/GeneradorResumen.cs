namespace LiqPagoEstandar.Core;

public static class GeneradorResumen
{
    public static ResumenPersonal Generar(
        Personal personal,
        Novedad novedad,
        ZonaDesfavorable zonaDesfavorable,
        DateOnly fechaActual,
        ParametrosLiquidacion parametros
    )
    {
        var sueldoBasicoNormal = CalculadoraLiquidacion.SueldoBasicoNormal(personal);
        var totalHorasNormales = CalculadoraLiquidacion.TotalHorasNormales(personal, novedad);
        var itemHorasExtras = CalculadoraLiquidacion.ItemHorasExtras(personal, novedad, parametros);
        var aniosAntiguedad = CalculadoraLiquidacion.AniosAntiguedad(personal, fechaActual);
        var itemAntiguedad = CalculadoraLiquidacion.ItemAntiguedad(sueldoBasicoNormal, aniosAntiguedad, parametros);
        var itemFeriados = CalculadoraLiquidacion.ItemFeriados(personal, novedad, parametros);
        var itemZonaDesfavorable = CalculadoraLiquidacion.ItemZonaDesfavorable(
            sueldoBasicoNormal,
            itemAntiguedad,
            personal,
            zonaDesfavorable,
            parametros
        );
        var totalAPagar = CalculadoraLiquidacion.TotalAPagar(
            totalHorasNormales,
            itemHorasExtras,
            itemAntiguedad,
            itemFeriados,
            itemZonaDesfavorable
        );

        return new ResumenPersonal(
            personal,
            novedad.Anio,
            novedad.Mes,
            sueldoBasicoNormal,
            totalHorasNormales,
            itemHorasExtras,
            aniosAntiguedad,
            itemAntiguedad,
            itemFeriados,
            itemZonaDesfavorable,
            totalAPagar
        );
    }

    public static IReadOnlyList<ResumenPersonal> GenerarParaMes(
        IEnumerable<Personal> personales,
        IEnumerable<Novedad> novedades,
        ZonaDesfavorable zonaDesfavorable,
        int anio,
        int mes,
        DateOnly fechaActual,
        ParametrosLiquidacion parametros
    )
    {
        var novedadesPorDni = novedades
            .Where(n => n.Anio == anio && n.Mes == mes)
            .ToDictionary(n => n.PersonalDni);

        return personales
            .Where(p => p.Activo && novedadesPorDni.ContainsKey(p.Dni))
            .Select(p => Generar(p, novedadesPorDni[p.Dni], zonaDesfavorable, fechaActual, parametros))
            .ToList();
    }
}
