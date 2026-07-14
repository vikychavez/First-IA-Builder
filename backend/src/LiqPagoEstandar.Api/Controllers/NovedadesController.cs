using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiqPagoEstandar.Api.Controllers;

[ApiController]
[Route("api/novedades")]
public class NovedadesController : ControllerBase
{
    private readonly INovedadService _novedadService;

    public NovedadesController(INovedadService novedadService)
    {
        _novedadService = novedadService;
    }

    [HttpGet]
    public async Task<ActionResult<List<NovedadDto>>> GetByPeriodo([FromQuery] int anio, [FromQuery] int mes)
    {
        return Ok(await _novedadService.GetByPeriodoAsync(anio, mes));
    }

    [HttpPut]
    public async Task<ActionResult<NovedadDto>> Upsert(NovedadUpsertRequest request)
    {
        return Ok(await _novedadService.UpsertAsync(request));
    }
}
