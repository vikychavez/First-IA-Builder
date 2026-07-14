using LiqPagoEstandar.Api.DTOs;

namespace LiqPagoEstandar.Api.Services;

public interface IClienteService
{
    Task<List<ClienteDto>> GetAllAsync(bool soloActivos);
    Task<ClienteDto?> GetByIdAsync(int id);
    Task<ClienteDto> CreateAsync(ClienteRequest request);
    Task<ClienteDto?> UpdateAsync(int id, ClienteRequest request);
    Task<bool> BajaAsync(int id);
}
