using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiqPagoEstandar.Api.Controllers;

[ApiController]
public class PersonalController : ControllerBase
{
    private readonly IPersonalService _personalService;

    public PersonalController(IPersonalService personalService)
    {
        _personalService = personalService;
    }

    [HttpGet("api/clientes/{clienteId:int}/personal")]
    public async Task<ActionResult<List<PersonalDto>>> GetByCliente(int clienteId, [FromQuery] bool soloActivos = true)
    {
        return Ok(await _personalService.GetByClienteAsync(clienteId, soloActivos));
    }

    [HttpGet("api/personal/{id:int}")]
    public async Task<ActionResult<PersonalDto>> GetById(int id)
    {
        var personal = await _personalService.GetByIdAsync(id);
        return personal is null ? NotFound() : Ok(personal);
    }

    [HttpPost("api/personal")]
    public async Task<ActionResult<PersonalDto>> Create(PersonalRequest request)
    {
        var respuesta = await _personalService.CreateAsync(request);
        return respuesta.Resultado switch
        {
            GuardarPersonalResultado.ClienteInexistente => BadRequest(new { mensaje = "El cliente indicado no existe." }),
            GuardarPersonalResultado.CategoriaInexistente => BadRequest(new { mensaje = "La categoría indicada no existe." }),
            GuardarPersonalResultado.DniDuplicado => BadRequest(new
            {
                mensaje = "El DNI es obligatorio y debe ser único entre los personales activos."
            }),
            _ => CreatedAtAction(nameof(GetById), new { id = respuesta.Personal!.Id }, respuesta.Personal)
        };
    }

    [HttpPut("api/personal/{id:int}")]
    public async Task<ActionResult<PersonalDto>> Update(int id, PersonalRequest request)
    {
        var respuesta = await _personalService.UpdateAsync(id, request);
        return respuesta.Resultado switch
        {
            GuardarPersonalResultado.ClienteInexistente => BadRequest(new { mensaje = "El cliente indicado no existe." }),
            GuardarPersonalResultado.CategoriaInexistente => BadRequest(new { mensaje = "La categoría indicada no existe." }),
            GuardarPersonalResultado.DniDuplicado => BadRequest(new
            {
                mensaje = "El DNI es obligatorio y debe ser único entre los personales activos."
            }),
            _ => respuesta.Personal is null ? NotFound() : Ok(respuesta.Personal)
        };
    }

    [HttpPost("api/personal/{id:int}/baja")]
    public async Task<IActionResult> Baja(int id)
    {
        var ok = await _personalService.BajaAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
