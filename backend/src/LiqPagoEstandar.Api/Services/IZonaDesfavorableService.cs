using LiqPagoEstandar.Api.DTOs;

namespace LiqPagoEstandar.Api.Services;

public enum GuardarZonaResultado
{
    Ok,
    ProvinciaDuplicada
}

public record GuardarZonaResponse(GuardarZonaResultado Resultado, ZonaDesfavorableDto? Zona);

public interface IZonaDesfavorableService
{
    Task<List<ZonaDesfavorableDto>> GetAllAsync(bool soloActivas);
    Task<GuardarZonaResponse> CreateAsync(ZonaDesfavorableRequest request);
    Task<GuardarZonaResponse> UpdateAsync(int id, ZonaDesfavorableRequest request);
    Task<bool> BajaAsync(int id);
}
