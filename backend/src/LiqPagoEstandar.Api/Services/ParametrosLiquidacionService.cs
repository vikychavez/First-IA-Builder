using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Data;
using LiqPagoEstandar.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LiqPagoEstandar.Api.Services;

public class ParametrosLiquidacionService : IParametrosLiquidacionService
{
    private readonly AppDbContext _db;

    public ParametrosLiquidacionService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ParametrosLiquidacionDto> GetActualAsync()
    {
        var parametros = await ObtenerEntidadAsync();
        return MapToDto(parametros);
    }

    public async Task<ParametrosLiquidacionDto> UpdateAsync(ParametrosLiquidacionRequest request)
    {
        var parametros = await ObtenerEntidadAsync();

        parametros.PorcentajeAntiguedad = request.PorcentajeAntiguedad!.Value;
        parametros.PorcentajeZonaDesfavorable = request.PorcentajeZonaDesfavorable!.Value;
        parametros.MultiplicadorHorasExtras = request.MultiplicadorHorasExtras!.Value;
        parametros.MultiplicadorFeriados = request.MultiplicadorFeriados!.Value;

        await _db.SaveChangesAsync();

        return MapToDto(parametros);
    }

    private async Task<Data.Entities.ParametrosLiquidacionEntity> ObtenerEntidadAsync() =>
        await _db.ParametrosLiquidacion.FirstAsync(p => p.Id == ParametrosLiquidacionConfiguration.IdUnico);

    private static ParametrosLiquidacionDto MapToDto(Data.Entities.ParametrosLiquidacionEntity p) =>
        new(p.PorcentajeAntiguedad, p.PorcentajeZonaDesfavorable, p.MultiplicadorHorasExtras, p.MultiplicadorFeriados);
}
