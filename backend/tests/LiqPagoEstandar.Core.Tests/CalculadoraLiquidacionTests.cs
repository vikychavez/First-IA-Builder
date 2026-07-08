using LiqPagoEstandar.Core;
using Xunit;

namespace LiqPagoEstandar.Core.Tests;

public class CalculadoraLiquidacionTests
{
    private static readonly Categoria Categoria = new("Niñera", ValorHoraConRetiro: 1000m, ValorHoraSinRetiro: 800m);

    private static Personal CrearPersonal(DateOnly fechaIngreso, string provincia = "Buenos Aires") =>
        new(
            Dni: "12345678",
            ClienteId: "cliente-1",
            FechaIngreso: fechaIngreso,
            Apellido: "Perez",
            Nombre: "Ana",
            Direccion: "Calle Falsa 123",
            Telefono: "1122334455",
            Categoria: Categoria,
            TipoRetiro: TipoRetiro.SinRetiro,
            Provincia: provincia,
            HorasMensualesPactadas: 192m
        );

    private static readonly Novedad Novedad = new(
        PersonalDni: "12345678",
        Anio: 2026,
        Mes: 7,
        HorasNormales: 180m,
        HorasFeriado: 8m,
        HorasExtra: 10m
    );

    [Fact] // AC-10
    public void SueldoBasicoNormal_EsHorasMensualesPorValorHora()
    {
        var personal = CrearPersonal(new DateOnly(2023, 7, 8));

        var resultado = CalculadoraLiquidacion.SueldoBasicoNormal(personal);

        Assert.Equal(153_600m, resultado);
    }

    [Fact] // AC-11
    public void TotalHorasNormales_EsHorasTrabajadasPorValorHora()
    {
        var personal = CrearPersonal(new DateOnly(2023, 7, 8));

        var resultado = CalculadoraLiquidacion.TotalHorasNormales(personal, Novedad);

        Assert.Equal(144_000m, resultado);
    }

    [Fact] // AC-12
    public void AniosAntiguedad_CalculaAniosCompletos()
    {
        var personal = CrearPersonal(new DateOnly(2023, 7, 8));
        var fechaActual = new DateOnly(2026, 7, 8);

        var resultado = CalculadoraLiquidacion.AniosAntiguedad(personal, fechaActual);

        Assert.Equal(3, resultado);
    }

    [Fact] // AC-12: un día antes del aniversario todavía no se completó el año
    public void AniosAntiguedad_NoCuentaAnioIncompleto()
    {
        var personal = CrearPersonal(new DateOnly(2023, 7, 8));
        var fechaActual = new DateOnly(2026, 7, 7);

        var resultado = CalculadoraLiquidacion.AniosAntiguedad(personal, fechaActual);

        Assert.Equal(2, resultado);
    }

    [Fact] // AC-13
    public void ItemAntiguedad_EsSueldoBasicoDivididoCienPorAnios()
    {
        var resultado = CalculadoraLiquidacion.ItemAntiguedad(sueldoBasicoNormal: 153_600m, aniosAntiguedad: 3);

        Assert.Equal(4_608m, resultado);
    }

    [Fact] // AC-14: provincia en ZonaDesfavorable
    public void ItemZonaDesfavorable_AplicaSiProvinciaEstaEnLaTabla()
    {
        var personal = CrearPersonal(new DateOnly(2023, 7, 8), provincia: "Chubut");
        var zonaDesfavorable = new ZonaDesfavorable(["Chubut", "Santa Cruz"]);

        var resultado = CalculadoraLiquidacion.ItemZonaDesfavorable(
            sueldoBasicoNormal: 153_600m,
            itemAntiguedad: 4_608m,
            personal,
            zonaDesfavorable
        );

        Assert.Equal(49_044.48m, resultado);
    }

    [Fact] // Personal cuya provincia NO figura en ZonaDesfavorable
    public void ItemZonaDesfavorable_NoAplicaSiProvinciaNoEstaEnLaTabla()
    {
        var personal = CrearPersonal(new DateOnly(2023, 7, 8), provincia: "Buenos Aires");
        var zonaDesfavorable = new ZonaDesfavorable(["Chubut", "Santa Cruz"]);

        var resultado = CalculadoraLiquidacion.ItemZonaDesfavorable(
            sueldoBasicoNormal: 153_600m,
            itemAntiguedad: 4_608m,
            personal,
            zonaDesfavorable
        );

        Assert.Equal(0m, resultado);
    }

    [Fact] // AC-15
    public void ItemHorasExtras_EsValorHoraPorUnoPuntoCincoPorCantidad()
    {
        var personal = CrearPersonal(new DateOnly(2023, 7, 8));

        var resultado = CalculadoraLiquidacion.ItemHorasExtras(personal, Novedad);

        Assert.Equal(12_000m, resultado);
    }

    [Fact] // AC-16
    public void ItemFeriados_EsValorHoraPorDosPorCantidad()
    {
        var personal = CrearPersonal(new DateOnly(2023, 7, 8));

        var resultado = CalculadoraLiquidacion.ItemFeriados(personal, Novedad);

        Assert.Equal(12_800m, resultado);
    }

    [Fact] // AC-17
    public void TotalAPagar_EsLaSumatoriaDeTodosLosItems()
    {
        var resultado = CalculadoraLiquidacion.TotalAPagar(
            totalHorasNormales: 144_000m,
            itemHorasExtras: 12_000m,
            itemAntiguedad: 4_608m,
            itemFeriados: 12_800m,
            itemZonaDesfavorable: 49_044.48m
        );

        Assert.Equal(222_452.48m, resultado);
    }
}
