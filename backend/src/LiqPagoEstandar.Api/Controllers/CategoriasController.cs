using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LiqPagoEstandar.Api.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaDto>>> GetAll([FromQuery] bool soloActivas = true)
    {
        return Ok(await _categoriaService.GetAllAsync(soloActivas));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaDto>> GetById(int id)
    {
        var categoria = await _categoriaService.GetByIdAsync(id);
        return categoria is null ? NotFound() : Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Create(CategoriaRequest request)
    {
        var categoria = await _categoriaService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDto>> Update(int id, CategoriaRequest request)
    {
        var categoria = await _categoriaService.UpdateAsync(id, request);
        return categoria is null ? NotFound() : Ok(categoria);
    }

    [HttpPost("{id:int}/baja")]
    public async Task<IActionResult> Baja(int id)
    {
        var ok = await _categoriaService.BajaAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
