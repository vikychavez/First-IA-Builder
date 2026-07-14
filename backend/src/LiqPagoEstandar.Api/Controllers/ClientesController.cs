using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Api.Services;
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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var cliente = await _clienteService.GetByIdAsync(id);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create(ClienteRequest request)
    {
        var cliente = await _clienteService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClienteDto>> Update(int id, ClienteRequest request)
    {
        var cliente = await _clienteService.UpdateAsync(id, request);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost("{id:int}/baja")]
    public async Task<IActionResult> Baja(int id)
    {
        var ok = await _clienteService.BajaAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
