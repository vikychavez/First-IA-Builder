using LiqPagoEstandar.Api.DTOs;

namespace LiqPagoEstandar.Api.Services;

public enum GuardarClienteResultado
{
    Ok,
    DniDuplicado
}

public record GuardarClienteResponse(GuardarClienteResultado Resultado, ClienteDto? Cliente);

public interface IClienteService
{
    Task<List<ClienteDto>> GetAllAsync(bool soloActivos);
    Task<ClienteDto?> GetByIdAsync(int id);
    Task<GuardarClienteResponse> CreateAsync(ClienteRequest request);
    Task<GuardarClienteResponse> UpdateAsync(int id, ClienteRequest request);
    Task<bool> BajaAsync(int id);
}
