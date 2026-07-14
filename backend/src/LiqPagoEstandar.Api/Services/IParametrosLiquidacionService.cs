using LiqPagoEstandar.Api.DTOs;

namespace LiqPagoEstandar.Api.Services;

public interface IParametrosLiquidacionService
{
    Task<ParametrosLiquidacionDto> GetActualAsync();
    Task<ParametrosLiquidacionDto> UpdateAsync(ParametrosLiquidacionRequest request);
}
