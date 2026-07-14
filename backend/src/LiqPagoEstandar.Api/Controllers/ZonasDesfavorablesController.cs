using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiqPagoEstandar.Api.Controllers;

[ApiController]
[Route("api/zonas-desfavorables")]
public class ZonasDesfavorablesController : ControllerBase
{
    private readonly IZonaDesfavorableService _zonaService;

    public ZonasDesfavorablesController(IZonaDesfavorableService zonaService)
    {
        _zonaService = zonaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ZonaDesfavorableDto>>> GetAll([FromQuery] bool soloActivas = true)
    {
        return Ok(await _zonaService.GetAllAsync(soloActivas));
    }

    [HttpPost]
    public async Task<ActionResult<ZonaDesfavorableDto>> Create(ZonaDesfavorableRequest request)
    {
        var respuesta = await _zonaService.CreateAsync(request);
        return respuesta.Resultado == GuardarZonaResultado.ProvinciaDuplicada
            ? BadRequest(new { mensaje = "La provincia ya está registrada y activa en ZonaDesfavorable." })
            : Ok(respuesta.Zona);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ZonaDesfavorableDto>> Update(int id, ZonaDesfavorableRequest request)
    {
        var respuesta = await _zonaService.UpdateAsync(id, request);
        return respuesta.Resultado switch
        {
            GuardarZonaResultado.ProvinciaDuplicada => BadRequest(new
            {
                mensaje = "La provincia ya está registrada y activa en ZonaDesfavorable."
            }),
            _ => respuesta.Zona is null ? NotFound() : Ok(respuesta.Zona)
        };
    }

    [HttpPost("{id:int}/baja")]
    public async Task<IActionResult> Baja(int id)
    {
        var ok = await _zonaService.BajaAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
