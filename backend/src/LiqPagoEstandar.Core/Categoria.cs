namespace LiqPagoEstandar.Core;

public record Categoria(string Nombre, decimal ValorHoraConRetiro, decimal ValorHoraSinRetiro)
{
    public decimal ValorHora(TipoRetiro tipoRetiro) =>
        tipoRetiro == TipoRetiro.ConRetiro ? ValorHoraConRetiro : ValorHoraSinRetiro;
}
