using LiqPagoEstandar.Api.DTOs;

namespace LiqPagoEstandar.Api.Services;

public enum GuardarPersonalResultado
{
    Ok,
    ClienteInexistente,
    CategoriaInexistente,
    DniDuplicado
}

public record GuardarPersonalResponse(GuardarPersonalResultado Resultado, PersonalDto? Personal);

public interface IPersonalService
{
    Task<List<PersonalDto>> GetByClienteAsync(int clienteId, bool soloActivos);
    Task<PersonalDto?> GetByIdAsync(int id);
    Task<GuardarPersonalResponse> CreateAsync(PersonalRequest request);
    Task<GuardarPersonalResponse> UpdateAsync(int id, PersonalRequest request);
    Task<bool> BajaAsync(int id);
}
