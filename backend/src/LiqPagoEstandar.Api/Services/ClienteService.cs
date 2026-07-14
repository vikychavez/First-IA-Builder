using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Data;
using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiqPagoEstandar.Api.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _db;

    public ClienteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ClienteDto>> GetAllAsync(bool soloActivos)
    {
        var query = _db.Clientes.AsNoTracking().AsQueryable();
        if (soloActivos)
        {
            query = query.Where(c => c.Activo);
        }

        return await query
            .OrderBy(c => c.Nombre)
            .Select(c => new ClienteDto(c.Id, c.Nombre, c.Email, c.Telefono, c.Direccion, c.Activo))
            .ToListAsync();
    }

    public async Task<ClienteDto?> GetByIdAsync(int id)
    {
        var cliente = await _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return cliente is null ? null : MapToDto(cliente);
    }

    public async Task<ClienteDto> CreateAsync(ClienteRequest request)
    {
        var cliente = new ClienteEntity
        {
            Nombre = request.Nombre,
            Email = request.Email,
            Telefono = request.Telefono,
            Direccion = request.Direccion,
            Activo = true
        };

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();

        return MapToDto(cliente);
    }

    public async Task<ClienteDto?> UpdateAsync(int id, ClienteRequest request)
    {
        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        if (cliente is null)
        {
            return null;
        }

        cliente.Nombre = request.Nombre;
        cliente.Email = request.Email;
        cliente.Telefono = request.Telefono;
        cliente.Direccion = request.Direccion;

        await _db.SaveChangesAsync();

        return MapToDto(cliente);
    }

    public async Task<bool> BajaAsync(int id)
    {
        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        if (cliente is null)
        {
            return false;
        }

        cliente.Activo = false;
        cliente.FechaBaja = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    private static ClienteDto MapToDto(ClienteEntity c) =>
        new(c.Id, c.Nombre, c.Email, c.Telefono, c.Direccion, c.Activo);
}
