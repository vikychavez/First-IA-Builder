using LiqPagoEstandar.Core;
using Xunit;

namespace LiqPagoEstandar.Core.Tests;

public class GeneradorResumenTests
{
    private static readonly Categoria Categoria = new("Niñera", ValorHoraConRetiro: 1000m, ValorHoraSinRetiro: 800m);
    private static readonly DateOnly FechaActual = new(2026, 7, 8);

    private static Personal CrearPersonal(string dni, string provincia) =>
        new(
            Dni: dni,
            ClienteId: "cliente-1",
            FechaIngreso: new DateOnly(2023, 7, 8),
            Apellido: "Perez",
            Nombre: "Ana",
            Direccion: "Calle Falsa 123",
            Telefono: "1122334455",
            Categoria: Categoria,
            TipoRetiro: TipoRetiro.SinRetiro,
            Provincia: provincia,
            HorasMensualesPactadas: 192m
        );

    private static Novedad CrearNovedad(string dni) =>
        new(PersonalDni: dni, Anio: 2026, Mes: 7, HorasNormales: 180m, HorasFeriado: 8m, HorasExtra: 10m);

    [Fact] // RF-03 + AC-17: personal en zona desfavorable
    public void Generar_ComponeTodosLosItemsYElTotal_ConZonaDesfavorable()
    {
        var personal = CrearPersonal("12345678", provincia: "Chubut");
        var novedad = CrearNovedad("12345678");
        var zonaDesfavorable = new ZonaDesfavorable(["Chubut"]);

        var resumen = GeneradorResumen.Generar(personal, novedad, zonaDesfavorable, FechaActual);

        Assert.Equal(153_600m, resumen.SueldoBasicoNormal);
        Assert.Equal(144_000m, resumen.TotalHorasNormales);
        Assert.Equal(12_000m, resumen.ItemHorasExtras);
        Assert.Equal(3, resumen.AniosAntiguedad);
        Assert.Equal(4_608m, resumen.ItemAntiguedad);
        Assert.Equal(12_800m, resumen.ItemFeriados);
        Assert.Equal(49_044.48m, resumen.ItemZonaDesfavorable);
        Assert.Equal(222_452.48m, resumen.TotalAPagar);
    }

    [Fact] // Personal cuya provincia no está en ZonaDesfavorable: el ítem no se aplica
    public void Generar_NoAplicaZonaDesfavorable_SiProvinciaNoFiguraEnLaTabla()
    {
        var personal = CrearPersonal("87654321", provincia: "Buenos Aires");
        var novedad = CrearNovedad("87654321");
        var zonaDesfavorable = new ZonaDesfavorable(["Chubut"]);

        var resumen = GeneradorResumen.Generar(personal, novedad, zonaDesfavorable, FechaActual);

        Assert.Equal(0m, resumen.ItemZonaDesfavorable);
        Assert.Equal(173_408m, resumen.TotalAPagar); // sin el item de zona desfavorable
    }

    [Fact] // RF-03: genera el resumen de todos los personales con novedades del mes seleccionado
    public void GenerarParaMes_DevuelveUnResumenPorCadaPersonalConNovedadesDelMes()
    {
        var personales = new[]
        {
            CrearPersonal("12345678", provincia: "Chubut"),
            CrearPersonal("87654321", provincia: "Buenos Aires"),
        };
        var novedades = new[]
        {
            CrearNovedad("12345678"),
            CrearNovedad("87654321"),
            new Novedad("12345678", Anio: 2026, Mes: 6, HorasNormales: 100m, HorasFeriado: 0m, HorasExtra: 0m), // otro mes, no debe incluirse
        };
        var zonaDesfavorable = new ZonaDesfavorable(["Chubut"]);

        var resumenes = GeneradorResumen.GenerarParaMes(
            personales,
            novedades,
            zonaDesfavorable,
            anio: 2026,
            mes: 7,
            FechaActual
        );

        Assert.Equal(2, resumenes.Count);
        Assert.All(resumenes, r => Assert.Equal(2026, r.Anio));
        Assert.All(resumenes, r => Assert.Equal(7, r.Mes));
    }
}
