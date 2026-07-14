using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiqPagoEstandar.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UsuarioDto>> Login(LoginRequest request)
    {
        var usuario = await _authService.LoginAsync(request);
        if (usuario is null)
        {
            return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos." });
        }

        return Ok(usuario);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return NoContent();
    }

    [HttpGet("me")]
    public async Task<ActionResult<UsuarioDto>> Me()
    {
        var usuario = await _authService.ObtenerUsuarioActualAsync();
        return usuario is null ? Unauthorized() : Ok(usuario);
    }
}
