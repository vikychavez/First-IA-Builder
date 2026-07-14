namespace LiqPagoEstandar.Data.Entities;

public class ResumenPersonalDetalleEntity
{
    public int Id { get; set; }
    public int ResumenMensualId { get; set; }
    public ResumenMensualEntity? ResumenMensual { get; set; }

    public int PersonalId { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public string PersonalNombreCompleto { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string CategoriaNombre { get; set; } = string.Empty;
    public decimal ValorHora { get; set; }

    public decimal SueldoBasicoNormal { get; set; }
    public decimal TotalHorasNormales { get; set; }
    public decimal ItemHorasExtras { get; set; }
    public int AniosAntiguedad { get; set; }
    public decimal ItemAntiguedad { get; set; }
    public decimal ItemFeriados { get; set; }
    public decimal ItemZonaDesfavorable { get; set; }
    public decimal TotalAPagar { get; set; }

    public byte[]? PdfContenido { get; set; }
    public string? PdfNombreArchivo { get; set; }
}
