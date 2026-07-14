using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Data;
using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiqPagoEstandar.Api.Services;

public class ZonaDesfavorableService : IZonaDesfavorableService
{
    private readonly AppDbContext _db;

    public ZonaDesfavorableService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ZonaDesfavorableDto>> GetAllAsync(bool soloActivas)
    {
        var query = _db.ZonasDesfavorables.AsNoTracking().AsQueryable();
        if (soloActivas)
        {
            query = query.Where(z => z.Activo);
        }

        return await query
            .OrderBy(z => z.Provincia)
            .Select(z => new ZonaDesfavorableDto(z.Id, z.Provincia, z.Activo))
            .ToListAsync();
    }

    public async Task<GuardarZonaResponse> CreateAsync(ZonaDesfavorableRequest request)
    {
        if (await ExisteProvinciaActivaAsync(request.Provincia, excluirId: null))
        {
            return new GuardarZonaResponse(GuardarZonaResultado.ProvinciaDuplicada, null);
        }

        var zona = new ZonaDesfavorableEntity { Provincia = request.Provincia, Activo = true };
        _db.ZonasDesfavorables.Add(zona);
        await _db.SaveChangesAsync();

        return new GuardarZonaResponse(GuardarZonaResultado.Ok, MapToDto(zona));
    }

    public async Task<GuardarZonaResponse> UpdateAsync(int id, ZonaDesfavorableRequest request)
    {
        var zona = await _db.ZonasDesfavorables.FirstOrDefaultAsync(z => z.Id == id);
        if (zona is null)
        {
            return new GuardarZonaResponse(GuardarZonaResultado.Ok, null);
        }

        if (await ExisteProvinciaActivaAsync(request.Provincia, excluirId: id))
        {
            return new GuardarZonaResponse(GuardarZonaResultado.ProvinciaDuplicada, null);
        }

        zona.Provincia = request.Provincia;
        await _db.SaveChangesAsync();

        return new GuardarZonaResponse(GuardarZonaResultado.Ok, MapToDto(zona));
    }

    public async Task<bool> BajaAsync(int id)
    {
        var zona = await _db.ZonasDesfavorables.FirstOrDefaultAsync(z => z.Id == id);
        if (zona is null)
        {
            return false;
        }

        zona.Activo = false;
        await _db.SaveChangesAsync();
        return true;
    }

    private Task<bool> ExisteProvinciaActivaAsync(string provincia, int? excluirId) =>
        _db.ZonasDesfavorables.AnyAsync(z => z.Provincia == provincia && z.Activo && z.Id != (excluirId ?? -1));

    private static ZonaDesfavorableDto MapToDto(ZonaDesfavorableEntity z) => new(z.Id, z.Provincia, z.Activo);
}
