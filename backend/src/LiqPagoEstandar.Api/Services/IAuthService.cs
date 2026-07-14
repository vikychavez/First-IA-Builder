using LiqPagoEstandar.Api.DTOs;

namespace LiqPagoEstandar.Api.Services;

public interface IAuthService
{
    Task<UsuarioDto?> LoginAsync(LoginRequest request);
    Task LogoutAsync();
    Task<UsuarioDto?> ObtenerUsuarioActualAsync();
}
