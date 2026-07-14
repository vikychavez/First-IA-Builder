using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Core;
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

        var clientes = await query.OrderBy(c => c.Apellido).ThenBy(c => c.Nombre).ToListAsync();
        return clientes.Select(MapToDto).ToList();
    }

    public async Task<ClienteDto?> GetByIdAsync(int id)
    {
        var cliente = await _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return cliente is null ? null : MapToDto(cliente);
    }

    public async Task<GuardarClienteResponse> CreateAsync(ClienteRequest request)
    {
        if (await ExisteDniActivoAsync(request.Dni, excluirId: null))
        {
            return new GuardarClienteResponse(GuardarClienteResultado.DniDuplicado, null);
        }

        var cliente = new ClienteEntity
        {
            Dni = request.Dni,
            Cuit = CalculadoraCuit.Calcular(request.Dni, request.Sexo),
            Sexo = request.Sexo,
            Apellido = request.Apellido,
            Nombre = request.Nombre,
            FechaNacimiento = request.FechaNacimiento!.Value,
            Email = request.Email,
            Telefono = request.Telefono,
            Direccion = request.Direccion,
            Activo = true
        };

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();

        return new GuardarClienteResponse(GuardarClienteResultado.Ok, MapToDto(cliente));
    }

    public async Task<GuardarClienteResponse> UpdateAsync(int id, ClienteRequest request)
    {
        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        if (cliente is null)
        {
            return new GuardarClienteResponse(GuardarClienteResultado.Ok, null);
        }

        if (await ExisteDniActivoAsync(request.Dni, excluirId: id))
        {
            return new GuardarClienteResponse(GuardarClienteResultado.DniDuplicado, null);
        }

        cliente.Dni = request.Dni;
        cliente.Cuit = CalculadoraCuit.Calcular(request.Dni, request.Sexo);
        cliente.Sexo = request.Sexo;
        cliente.Apellido = request.Apellido;
        cliente.Nombre = request.Nombre;
        cliente.FechaNacimiento = request.FechaNacimiento!.Value;
        cliente.Email = request.Email;
        cliente.Telefono = request.Telefono;
        cliente.Direccion = request.Direccion;

        await _db.SaveChangesAsync();

        return new GuardarClienteResponse(GuardarClienteResultado.Ok, MapToDto(cliente));
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

    private Task<bool> ExisteDniActivoAsync(string dni, int? excluirId) =>
        _db.Clientes.AnyAsync(c => c.Dni == dni && c.Activo && c.Id != (excluirId ?? -1));

    private static ClienteDto MapToDto(ClienteEntity c) =>
        new(c.Id, c.Dni, c.Cuit, c.Sexo, c.Apellido, c.Nombre, c.FechaNacimiento, c.Email, c.Telefono, c.Direccion, c.Activo);
}
