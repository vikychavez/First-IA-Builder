using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Data;
using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiqPagoEstandar.Api.Services;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _db;

    public CategoriaService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CategoriaDto>> GetAllAsync(bool soloActivas)
    {
        var query = _db.Categorias.AsNoTracking().AsQueryable();
        if (soloActivas)
        {
            query = query.Where(c => c.Activo);
        }

        return await query
            .OrderBy(c => c.Nombre)
            .Select(c => new CategoriaDto(c.Id, c.Nombre, c.ValorHoraConRetiro, c.ValorHoraSinRetiro, c.Activo))
            .ToListAsync();
    }

    public async Task<CategoriaDto?> GetByIdAsync(int id)
    {
        var categoria = await _db.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return categoria is null ? null : MapToDto(categoria);
    }

    public async Task<CategoriaDto> CreateAsync(CategoriaRequest request)
    {
        var categoria = new CategoriaEntity
        {
            Nombre = request.Nombre,
            ValorHoraConRetiro = request.ValorHoraConRetiro!.Value,
            ValorHoraSinRetiro = request.ValorHoraSinRetiro!.Value,
            Activo = true
        };

        _db.Categorias.Add(categoria);
        await _db.SaveChangesAsync();

        return MapToDto(categoria);
    }

    public async Task<CategoriaDto?> UpdateAsync(int id, CategoriaRequest request)
    {
        var categoria = await _db.Categorias.FirstOrDefaultAsync(c => c.Id == id);
        if (categoria is null)
        {
            return null;
        }

        categoria.Nombre = request.Nombre;
        categoria.ValorHoraConRetiro = request.ValorHoraConRetiro!.Value;
        categoria.ValorHoraSinRetiro = request.ValorHoraSinRetiro!.Value;

        await _db.SaveChangesAsync();

        return MapToDto(categoria);
    }

    public async Task<bool> BajaAsync(int id)
    {
        var categoria = await _db.Categorias.FirstOrDefaultAsync(c => c.Id == id);
        if (categoria is null)
        {
            return false;
        }

        categoria.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }

    private static CategoriaDto MapToDto(CategoriaEntity c) =>
        new(c.Id, c.Nombre, c.ValorHoraConRetiro, c.ValorHoraSinRetiro, c.Activo);
}
