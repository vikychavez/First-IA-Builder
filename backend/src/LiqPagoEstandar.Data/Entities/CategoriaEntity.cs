using CoreCategoria = LiqPagoEstandar.Core.Categoria;

namespace LiqPagoEstandar.Data.Entities;

public class CategoriaEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal ValorHoraConRetiro { get; set; }
    public decimal ValorHoraSinRetiro { get; set; }
    public bool Activo { get; set; } = true;

    public CoreCategoria ToCore() => new(Nombre, ValorHoraConRetiro, ValorHoraSinRetiro);
}
