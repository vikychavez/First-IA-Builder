using LiqPagoEstandar.Data.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LiqPagoEstandar.Api.Services;

public class PdfService : IPdfService
{
    private static readonly string[] NombresMeses =
    [
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
    ];

    public byte[] GenerarPdf(ResumenPersonalDetalleEntity detalle, int anio, int mes)
    {
        var periodo = $"{NombresMeses[mes - 1]} {anio}";

        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(column =>
                {
                    column.Item().Text("Resumen de Pago").FontSize(18).Bold();
                    column.Item().Text($"Período: {periodo}");
                    column.Item().PaddingTop(10).Text($"Nombre: {detalle.PersonalNombreCompleto}");
                    column.Item().Text($"DNI: {detalle.Dni}");
                    column.Item().Text($"Categoría: {detalle.CategoriaNombre}");
                    column.Item().Text($"Valor Hora: {detalle.ValorHora:N2}");
                });

                page.Content().PaddingTop(20).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(1);
                    });

                    void Fila(string etiqueta, decimal valor)
                    {
                        table.Cell().Text(etiqueta);
                        table.Cell().AlignRight().Text(valor.ToString("N2"));
                    }

                    Fila("Horas Normales", detalle.TotalHorasNormales);
                    Fila("Horas Extras", detalle.ItemHorasExtras);
                    Fila("Antigüedad", detalle.ItemAntiguedad);
                    Fila("Feriados", detalle.ItemFeriados);
                    Fila("Zona Desfavorable", detalle.ItemZonaDesfavorable);
                });

                page.Footer().PaddingTop(20).Row(row =>
                {
                    row.RelativeItem().Text("Total a Pagar").Bold();
                    row.RelativeItem().AlignRight().Text(detalle.TotalAPagar.ToString("N2")).Bold();
                });
            });
        });

        return documento.GeneratePdf();
    }

    public string NombreArchivo(ResumenPersonalDetalleEntity detalle, int anio, int mes)
    {
        var nombre = detalle.PersonalNombreCompleto
            .Replace(",", string.Empty)
            .Replace(" ", "_");

        return $"{nombre}_{mes:D2}_{anio}.pdf";
    }
}
