using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiqPagoEstandar.Api.Controllers;

[ApiController]
[Route("api/resumenes")]
public class ResumenController : ControllerBase
{
    private readonly IResumenService _resumenService;

    public ResumenController(IResumenService resumenService)
    {
        _resumenService = resumenService;
    }

    [HttpGet]
    public async Task<ActionResult<ResumenMensualDto>> Get([FromQuery] int anio, [FromQuery] int mes)
    {
        var resumen = await _resumenService.GetAsync(anio, mes);
        return resumen is null ? NotFound() : Ok(resumen);
    }

    [HttpPost("generar")]
    public async Task<ActionResult<ResumenMensualDto>> Generar(GenerarResumenRequest request)
    {
        var resumen = await _resumenService.GenerarAsync(request.Anio!.Value, request.Mes!.Value);
        return Ok(resumen);
    }

    [HttpGet("{anio:int}/{mes:int}/personal/{personalId:int}/pdf")]
    public async Task<IActionResult> DescargarPdf(int anio, int mes, int personalId)
    {
        var pdf = await _resumenService.GetPdfAsync(anio, mes, personalId);
        return pdf is null ? NotFound() : File(pdf.Value.Contenido, "application/pdf", pdf.Value.NombreArchivo);
    }

    [HttpPost("enviar")]
    public async Task<ActionResult<EnviarResumenResultado>> Enviar(GenerarResumenRequest request)
    {
        try
        {
            var resultado = await _resumenService.EnviarAsync(request.Anio!.Value, request.Mes!.Value);
            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
