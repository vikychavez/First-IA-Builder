using CoreParametros = LiqPagoEstandar.Core.ParametrosLiquidacion;

namespace LiqPagoEstandar.Data.Entities;

public class ParametrosLiquidacionEntity
{
    public int Id { get; set; }
    public decimal PorcentajeAntiguedad { get; set; }
    public decimal PorcentajeZonaDesfavorable { get; set; }
    public decimal MultiplicadorHorasExtras { get; set; }
    public decimal MultiplicadorFeriados { get; set; }

    public CoreParametros ToCore() =>
        new(PorcentajeAntiguedad, PorcentajeZonaDesfavorable, MultiplicadorHorasExtras, MultiplicadorFeriados);
}
