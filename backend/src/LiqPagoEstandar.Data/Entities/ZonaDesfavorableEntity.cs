namespace LiqPagoEstandar.Data.Entities;

public class ZonaDesfavorableEntity
{
    public int Id { get; set; }
    public string Provincia { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}
