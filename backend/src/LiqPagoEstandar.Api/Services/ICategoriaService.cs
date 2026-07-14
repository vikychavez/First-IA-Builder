using LiqPagoEstandar.Api.DTOs;

namespace LiqPagoEstandar.Api.Services;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> GetAllAsync(bool soloActivas);
    Task<CategoriaDto?> GetByIdAsync(int id);
    Task<CategoriaDto> CreateAsync(CategoriaRequest request);
    Task<CategoriaDto?> UpdateAsync(int id, CategoriaRequest request);
    Task<bool> BajaAsync(int id);
}
