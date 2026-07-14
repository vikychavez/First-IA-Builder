using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Api.Services;
using LiqPagoEstandar.Core;
using Microsoft.AspNetCore.Mvc;

namespace LiqPagoEstandar.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClienteDto>>> GetAll([FromQuery] bool soloActivos = true)
    {
        return Ok(await _clienteService.GetAllAsync(soloActivos));
    }

    [HttpGet("calcular-cuit")]
    public ActionResult<object> CalcularCuit([FromQuery] string dni, [FromQuery] string sexo)
    {
        try
        {
            return Ok(new { cuit = CalculadoraCuit.Calcular(dni, sexo) });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var cliente = await _clienteService.GetByIdAsync(id);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create(ClienteRequest request)
    {
        var respuesta = await _clienteService.CreateAsync(request);
        return respuesta.Resultado switch
        {
            GuardarClienteResultado.DniDuplicado => BadRequest(new
            {
                mensaje = "El DNI es obligatorio y debe ser único entre los clientes activos."
            }),
            _ => CreatedAtAction(nameof(GetById), new { id = respuesta.Cliente!.Id }, respuesta.Cliente)
        };
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClienteDto>> Update(int id, ClienteRequest request)
    {
        var respuesta = await _clienteService.UpdateAsync(id, request);
        return respuesta.Resultado switch
        {
            GuardarClienteResultado.DniDuplicado => BadRequest(new
            {
                mensaje = "El DNI es obligatorio y debe ser único entre los clientes activos."
            }),
            _ => respuesta.Cliente is null ? NotFound() : Ok(respuesta.Cliente)
        };
    }

    [HttpPost("{id:int}/baja")]
    public async Task<IActionResult> Baja(int id)
    {
        var ok = await _clienteService.BajaAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
