using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Data;
using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiqPagoEstandar.Api.Services;

public class NovedadService : INovedadService
{
    private readonly AppDbContext _db;

    public NovedadService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<NovedadDto>> GetByPeriodoAsync(int anio, int mes)
    {
        var personalActivo = await _db.Personal
            .AsNoTracking()
            .Include(p => p.Cliente)
            .Where(p => p.Activo)
            .OrderBy(p => p.Apellido)
            .ThenBy(p => p.Nombre)
            .ToListAsync();

        var novedadesDelPeriodo = await _db.Novedades
            .AsNoTracking()
            .Where(n => n.Anio == anio && n.Mes == mes)
            .ToDictionaryAsync(n => n.PersonalId);

        return personalActivo
            .Select(p =>
            {
                novedadesDelPeriodo.TryGetValue(p.Id, out var novedad);
                return new NovedadDto(
                    p.Id,
                    p.Dni,
                    p.Cliente is null ? string.Empty : $"{p.Cliente.Apellido}, {p.Cliente.Nombre}",
                    $"{p.Apellido}, {p.Nombre}",
                    novedad?.HorasNormales ?? 0m,
                    novedad?.HorasFeriado ?? 0m,
                    novedad?.HorasExtra ?? 0m
                );
            })
            .ToList();
    }

    public async Task<NovedadDto> UpsertAsync(NovedadUpsertRequest request)
    {
        var personal = await _db.Personal
            .Include(p => p.Cliente)
            .FirstOrDefaultAsync(p => p.Id == request.PersonalId)
            ?? throw new InvalidOperationException("El personal indicado no existe.");

        var novedad = await _db.Novedades.FirstOrDefaultAsync(n =>
            n.PersonalId == request.PersonalId && n.Anio == request.Anio && n.Mes == request.Mes);

        if (novedad is null)
        {
            novedad = new NovedadEntity
            {
                PersonalId = request.PersonalId,
                Anio = request.Anio!.Value,
                Mes = request.Mes!.Value
            };
            _db.Novedades.Add(novedad);
        }

        novedad.HorasNormales = request.HorasNormales!.Value;
        novedad.HorasFeriado = request.HorasFeriado!.Value;
        novedad.HorasExtra = request.HorasExtra!.Value;
        novedad.FechaActualizacion = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return new NovedadDto(
            personal.Id,
            personal.Dni,
            personal.Cliente is null ? string.Empty : $"{personal.Cliente.Apellido}, {personal.Cliente.Nombre}",
            $"{personal.Apellido}, {personal.Nombre}",
            novedad.HorasNormales,
            novedad.HorasFeriado,
            novedad.HorasExtra
        );
    }
}
