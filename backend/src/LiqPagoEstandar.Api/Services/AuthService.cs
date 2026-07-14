using System.Security.Claims;
using LiqPagoEstandar.Api.Auth;
using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace LiqPagoEstandar.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(AppDbContext db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UsuarioDto?> LoginAsync(LoginRequest request)
    {
        var usuario = await _db.Usuarios
            .FirstOrDefaultAsync(u => u.NombreUsuario == request.NombreUsuario && u.Activo);

        if (usuario is null || !PasswordHasher.Verify(request.Password, usuario.PasswordHash))
        {
            return null;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.NombreUsuario)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return new UsuarioDto(usuario.Id, usuario.NombreUsuario);
    }

    public async Task LogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public async Task<UsuarioDto?> ObtenerUsuarioActualAsync()
    {
        var idClaim = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (idClaim is null || !int.TryParse(idClaim, out var id))
        {
            return null;
        }

        var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id && u.Activo);
        return usuario is null ? null : new UsuarioDto(usuario.Id, usuario.NombreUsuario);
    }

    private HttpContext HttpContext =>
        _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("No hay un HttpContext activo.");
}
