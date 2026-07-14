using System.ComponentModel.DataAnnotations;
using LiqPagoEstandar.Core;

namespace LiqPagoEstandar.Api.DTOs;

public record PersonalDto(
    int Id,
    string Dni,
    int ClienteId,
    string ClienteNombre,
    DateOnly FechaIngreso,
    string Apellido,
    string Nombre,
    string Direccion,
    string Telefono,
    int CategoriaId,
    string CategoriaNombre,
    TipoRetiro TipoRetiro,
    string Provincia,
    decimal HorasMensualesPactadas,
    decimal ValorHoraBase,
    bool Activo
);

public class PersonalRequest
{
    [Required]
    public string Dni { get; set; } = string.Empty;

    [Required]
    public int ClienteId { get; set; }

    [Required]
    public DateOnly? FechaIngreso { get; set; }

    [Required]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    public string Telefono { get; set; } = string.Empty;

    [Required]
    public int CategoriaId { get; set; }

    [Required]
    public TipoRetiro? TipoRetiro { get; set; }

    [Required]
    public string Provincia { get; set; } = string.Empty;

    [Required]
    public decimal? HorasMensualesPactadas { get; set; }
}
