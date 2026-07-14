using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Data;
using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiqPagoEstandar.Api.Services;

public class PersonalService : IPersonalService
{
    private readonly AppDbContext _db;

    public PersonalService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<PersonalDto>> GetByClienteAsync(int clienteId, bool soloActivos)
    {
        var query = _db.Personal
            .AsNoTracking()
            .Include(p => p.Cliente)
            .Include(p => p.Categoria)
            .Where(p => p.ClienteId == clienteId);

        if (soloActivos)
        {
            query = query.Where(p => p.Activo);
        }

        var personal = await query.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre).ToListAsync();
        return personal.Select(MapToDto).ToList();
    }

    public async Task<PersonalDto?> GetByIdAsync(int id)
    {
        var personal = await _db.Personal
            .AsNoTracking()
            .Include(p => p.Cliente)
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        return personal is null ? null : MapToDto(personal);
    }

    public async Task<GuardarPersonalResponse> CreateAsync(PersonalRequest request)
    {
        if (!await _db.Clientes.AnyAsync(c => c.Id == request.ClienteId))
        {
            return new GuardarPersonalResponse(GuardarPersonalResultado.ClienteInexistente, null);
        }

        if (!await _db.Categorias.AnyAsync(c => c.Id == request.CategoriaId))
        {
            return new GuardarPersonalResponse(GuardarPersonalResultado.CategoriaInexistente, null);
        }

        if (await ExisteDniActivoAsync(request.Dni, excluirId: null))
        {
            return new GuardarPersonalResponse(GuardarPersonalResultado.DniDuplicado, null);
        }

        var personal = new PersonalEntity
        {
            Dni = request.Dni,
            ClienteId = request.ClienteId,
            FechaIngreso = request.FechaIngreso!.Value,
            Apellido = request.Apellido,
            Nombre = request.Nombre,
            Direccion = request.Direccion,
            Telefono = request.Telefono,
            CategoriaId = request.CategoriaId,
            TipoRetiro = request.TipoRetiro!.Value,
            Provincia = request.Provincia,
            HorasMensualesPactadas = request.HorasMensualesPactadas!.Value,
            Activo = true
        };

        _db.Personal.Add(personal);
        await _db.SaveChangesAsync();

        return new GuardarPersonalResponse(GuardarPersonalResultado.Ok, await GetByIdAsync(personal.Id));
    }

    public async Task<GuardarPersonalResponse> UpdateAsync(int id, PersonalRequest request)
    {
        var personal = await _db.Personal.FirstOrDefaultAsync(p => p.Id == id);
        if (personal is null)
        {
            return new GuardarPersonalResponse(GuardarPersonalResultado.Ok, null);
        }

        if (!await _db.Clientes.AnyAsync(c => c.Id == request.ClienteId))
        {
            return new GuardarPersonalResponse(GuardarPersonalResultado.ClienteInexistente, null);
        }

        if (!await _db.Categorias.AnyAsync(c => c.Id == request.CategoriaId))
        {
            return new GuardarPersonalResponse(GuardarPersonalResultado.CategoriaInexistente, null);
        }

        if (await ExisteDniActivoAsync(request.Dni, excluirId: id))
        {
            return new GuardarPersonalResponse(GuardarPersonalResultado.DniDuplicado, null);
        }

        personal.Dni = request.Dni;
        personal.ClienteId = request.ClienteId;
        personal.FechaIngreso = request.FechaIngreso!.Value;
        personal.Apellido = request.Apellido;
        personal.Nombre = request.Nombre;
        personal.Direccion = request.Direccion;
        personal.Telefono = request.Telefono;
        personal.CategoriaId = request.CategoriaId;
        personal.TipoRetiro = request.TipoRetiro!.Value;
        personal.Provincia = request.Provincia;
        personal.HorasMensualesPactadas = request.HorasMensualesPactadas!.Value;

        await _db.SaveChangesAsync();

        return new GuardarPersonalResponse(GuardarPersonalResultado.Ok, await GetByIdAsync(id));
    }

    public async Task<bool> BajaAsync(int id)
    {
        var personal = await _db.Personal.FirstOrDefaultAsync(p => p.Id == id);
        if (personal is null)
        {
            return false;
        }

        personal.Activo = false;
        personal.FechaBaja = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    private Task<bool> ExisteDniActivoAsync(string dni, int? excluirId) =>
        _db.Personal.AnyAsync(p => p.Dni == dni && p.Activo && p.Id != (excluirId ?? -1));

    private static PersonalDto MapToDto(PersonalEntity p) =>
        new(
            p.Id,
            p.Dni,
            p.ClienteId,
            p.Cliente is null ? string.Empty : $"{p.Cliente.Apellido}, {p.Cliente.Nombre}",
            p.FechaIngreso,
            p.Apellido,
            p.Nombre,
            p.Direccion,
            p.Telefono,
            p.CategoriaId,
            p.Categoria?.Nombre ?? string.Empty,
            p.TipoRetiro,
            p.Provincia,
            p.HorasMensualesPactadas,
            p.Categoria?.ToCore().ValorHora(p.TipoRetiro) ?? 0m,
            p.Activo
        );
}
