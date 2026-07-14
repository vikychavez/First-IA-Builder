namespace LiqPagoEstandar.Data.Entities;

public class NovedadEntity
{
    public int Id { get; set; }
    public int PersonalId { get; set; }
    public PersonalEntity? Personal { get; set; }
    public int Anio { get; set; }
    public int Mes { get; set; }
    public decimal HorasNormales { get; set; }
    public decimal HorasFeriado { get; set; }
    public decimal HorasExtra { get; set; }
    public DateTime FechaActualizacion { get; set; }
}
