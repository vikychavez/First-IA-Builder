namespace LiqPagoEstandar.Core;

public record Personal(
    string Dni,
    string ClienteId,
    DateOnly FechaIngreso,
    string Apellido,
    string Nombre,
    string Direccion,
    string Telefono,
    Categoria Categoria,
    TipoRetiro TipoRetiro,
    string Provincia,
    decimal HorasMensualesPactadas
)
{
    public decimal ValorHoraBase => Categoria.ValorHora(TipoRetiro);
}
