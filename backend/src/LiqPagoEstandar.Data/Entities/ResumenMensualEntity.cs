namespace LiqPagoEstandar.Data.Entities;

public class ResumenMensualEntity
{
    public int Id { get; set; }
    public int Anio { get; set; }
    public int Mes { get; set; }
    public EstadoResumen Estado { get; set; }
    public DateTime FechaGeneracion { get; set; }
    public DateTime? FechaEnvio { get; set; }

    public List<ResumenPersonalDetalleEntity> Detalles { get; set; } = [];
}
