using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiqPagoEstandar.Api.Controllers;

[ApiController]
[Route("api/parametros-liquidacion")]
public class ParametrosController : ControllerBase
{
    private readonly IParametrosLiquidacionService _parametrosService;

    public ParametrosController(IParametrosLiquidacionService parametrosService)
    {
        _parametrosService = parametrosService;
    }

    [HttpGet]
    public async Task<ActionResult<ParametrosLiquidacionDto>> Get()
    {
        return Ok(await _parametrosService.GetActualAsync());
    }

    [HttpPut]
    public async Task<ActionResult<ParametrosLiquidacionDto>> Update(ParametrosLiquidacionRequest request)
    {
        return Ok(await _parametrosService.UpdateAsync(request));
    }
}
