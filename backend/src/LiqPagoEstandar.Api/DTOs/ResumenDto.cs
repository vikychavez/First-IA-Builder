using System.ComponentModel.DataAnnotations;

namespace LiqPagoEstandar.Api.DTOs;

public record ResumenPersonalDetalleDto(
    int PersonalId,
    int ClienteId,
    string ClienteNombre,
    string PersonalNombreCompleto,
    string Dni,
    string CategoriaNombre,
    decimal ValorHora,
    decimal SueldoBasicoNormal,
    decimal TotalHorasNormales,
    decimal ItemHorasExtras,
    int AniosAntiguedad,
    decimal ItemAntiguedad,
    decimal ItemFeriados,
    decimal ItemZonaDesfavorable,
    decimal TotalAPagar,
    bool TienePdf
);

public record ResumenMensualDto(
    int Anio,
    int Mes,
    string Estado,
    DateTime FechaGeneracion,
    DateTime? FechaEnvio,
    List<ResumenPersonalDetalleDto> Detalles
);

public class GenerarResumenRequest
{
    [Required]
    public int? Anio { get; set; }

    [Required]
    public int? Mes { get; set; }
}

public record EnviarResumenResultado(
    List<string> ClientesEnviados,
    List<string> ClientesSinEmail,
    List<string> ClientesConError
);
