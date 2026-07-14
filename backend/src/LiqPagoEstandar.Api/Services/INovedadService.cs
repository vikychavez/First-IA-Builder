using LiqPagoEstandar.Api.DTOs;

namespace LiqPagoEstandar.Api.Services;

public interface INovedadService
{
    Task<List<NovedadDto>> GetByPeriodoAsync(int anio, int mes);
    Task<NovedadDto> UpsertAsync(NovedadUpsertRequest request);
}
