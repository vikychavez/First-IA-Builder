using LiqPagoEstandar.Api.DTOs;
using LiqPagoEstandar.Core;
using LiqPagoEstandar.Data;
using LiqPagoEstandar.Data.Configurations;
using LiqPagoEstandar.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiqPagoEstandar.Api.Services;

public class ResumenService : IResumenService
{
    private readonly AppDbContext _db;
    private readonly IPdfService _pdfService;
    private readonly IEmailService _emailService;

    public ResumenService(AppDbContext db, IPdfService pdfService, IEmailService emailService)
    {
        _db = db;
        _pdfService = pdfService;
        _emailService = emailService;
    }

    public async Task<ResumenMensualDto> GenerarAsync(int anio, int mes)
    {
        var parametros = (await _db.ParametrosLiquidacion
            .AsNoTracking()
            .FirstAsync(p => p.Id == ParametrosLiquidacionConfiguration.IdUnico))
            .ToCore();

        var personalEntities = await _db.Personal
            .AsNoTracking()
            .Include(p => p.Cliente)
            .Include(p => p.Categoria)
            .Where(p => p.Activo)
            .ToListAsync();

        var personalPorId = personalEntities.ToDictionary(p => p.Id);
        var personalesCore = personalEntities.Select(p => p.ToCore()).ToList();

        var novedadesCore = (await _db.Novedades
            .AsNoTracking()
            .Where(n => n.Anio == anio && n.Mes == mes)
            .ToListAsync())
            .Where(n => personalPorId.ContainsKey(n.PersonalId))
            .Select(n => new Novedad(
                personalPorId[n.PersonalId].Dni,
                n.Anio,
                n.Mes,
                n.HorasNormales,
                n.HorasFeriado,
                n.HorasExtra))
            .ToList();

        var provinciasActivas = await _db.ZonasDesfavorables
            .AsNoTracking()
            .Where(z => z.Activo)
            .Select(z => z.Provincia)
            .ToListAsync();
        var zonaDesfavorable = new ZonaDesfavorable(provinciasActivas);

        var resumenes = GeneradorResumen.GenerarParaMes(
            personalesCore,
            novedadesCore,
            zonaDesfavorable,
            anio,
            mes,
            DateOnly.FromDateTime(DateTime.UtcNow),
            parametros
        );

        // RF-31: un nuevo resumen del mismo período reemplaza al anterior.
        var existente = await _db.ResumenesMensuales.FirstOrDefaultAsync(r => r.Anio == anio && r.Mes == mes);
        if (existente is not null)
        {
            _db.ResumenesMensuales.Remove(existente);
            await _db.SaveChangesAsync();
        }

        var resumenMensual = new ResumenMensualEntity
        {
            Anio = anio,
            Mes = mes,
            Estado = EstadoResumen.Generado,
            FechaGeneracion = DateTime.UtcNow
        };

        foreach (var r in resumenes)
        {
            var personalEntity = personalPorId.Values.First(p => p.Dni == r.Personal.Dni);
            var detalle = new ResumenPersonalDetalleEntity
            {
                PersonalId = personalEntity.Id,
                ClienteId = personalEntity.ClienteId,
                ClienteNombre = personalEntity.Cliente is null ? string.Empty : $"{personalEntity.Cliente.Apellido}, {personalEntity.Cliente.Nombre}",
                PersonalNombreCompleto = $"{personalEntity.Apellido}, {personalEntity.Nombre}",
                Dni = personalEntity.Dni,
                CategoriaNombre = personalEntity.Categoria?.Nombre ?? string.Empty,
                ValorHora = r.Personal.ValorHoraBase,
                SueldoBasicoNormal = r.SueldoBasicoNormal,
                TotalHorasNormales = r.TotalHorasNormales,
                ItemHorasExtras = r.ItemHorasExtras,
                AniosAntiguedad = r.AniosAntiguedad,
                ItemAntiguedad = r.ItemAntiguedad,
                ItemFeriados = r.ItemFeriados,
                ItemZonaDesfavorable = r.ItemZonaDesfavorable,
                TotalAPagar = r.TotalAPagar
            };

            // RF-29/RF-32: el PDF se genera junto con el resumen (AC-37/AC-38/AC-42).
            detalle.PdfContenido = _pdfService.GenerarPdf(detalle, anio, mes);
            detalle.PdfNombreArchivo = _pdfService.NombreArchivo(detalle, anio, mes);

            resumenMensual.Detalles.Add(detalle);
        }

        _db.ResumenesMensuales.Add(resumenMensual);
        await _db.SaveChangesAsync();

        return MapToDto(resumenMensual);
    }

    public async Task<ResumenMensualDto?> GetAsync(int anio, int mes)
    {
        var resumen = await _db.ResumenesMensuales
            .AsNoTracking()
            .Include(r => r.Detalles)
            .FirstOrDefaultAsync(r => r.Anio == anio && r.Mes == mes);

        return resumen is null ? null : MapToDto(resumen);
    }

    public async Task<(byte[] Contenido, string NombreArchivo)?> GetPdfAsync(int anio, int mes, int personalId)
    {
        var detalle = await _db.ResumenPersonalDetalles
            .AsNoTracking()
            .Include(d => d.ResumenMensual)
            .FirstOrDefaultAsync(d =>
                d.PersonalId == personalId &&
                d.ResumenMensual!.Anio == anio &&
                d.ResumenMensual.Mes == mes);

        if (detalle?.PdfContenido is null || detalle.PdfNombreArchivo is null)
        {
            return null;
        }

        return (detalle.PdfContenido, detalle.PdfNombreArchivo);
    }

    public async Task<EnviarResumenResultado> EnviarAsync(int anio, int mes)
    {
        var resumen = await _db.ResumenesMensuales
            .Include(r => r.Detalles)
            .FirstOrDefaultAsync(r => r.Anio == anio && r.Mes == mes)
            ?? throw new InvalidOperationException("No hay un resumen generado para el período indicado.");

        var clienteIds = resumen.Detalles.Select(d => d.ClienteId).Distinct().ToList();
        var clientes = await _db.Clientes
            .AsNoTracking()
            .Where(c => clienteIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id);

        var enviados = new List<string>();
        var sinEmail = new List<string>();
        var conError = new List<string>();

        foreach (var grupo in resumen.Detalles.GroupBy(d => d.ClienteId))
        {
            var clienteNombre = grupo.First().ClienteNombre;
            var email = clientes.TryGetValue(grupo.Key, out var cliente) ? cliente.Email : null;

            if (string.IsNullOrWhiteSpace(email))
            {
                sinEmail.Add(clienteNombre);
                continue;
            }

            var adjuntos = grupo
                .Where(d => d.PdfContenido is not null && d.PdfNombreArchivo is not null)
                .Select(d => new AdjuntoEmail(d.PdfNombreArchivo!, d.PdfContenido!))
                .ToList();

            try
            {
                await _emailService.EnviarResumenAsync(email, clienteNombre, anio, mes, adjuntos);
                enviados.Add(clienteNombre);
            }
            catch
            {
                conError.Add(clienteNombre);
            }
        }

        if (enviados.Count > 0)
        {
            resumen.Estado = EstadoResumen.Enviado;
            resumen.FechaEnvio = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        return new EnviarResumenResultado(enviados, sinEmail, conError);
    }

    private static ResumenMensualDto MapToDto(ResumenMensualEntity resumen) =>
        new(
            resumen.Anio,
            resumen.Mes,
            resumen.Estado.ToString(),
            resumen.FechaGeneracion,
            resumen.FechaEnvio,
            resumen.Detalles
                .OrderBy(d => d.ClienteNombre)
                .ThenBy(d => d.PersonalNombreCompleto)
                .Select(d => new ResumenPersonalDetalleDto(
                    d.PersonalId,
                    d.ClienteId,
                    d.ClienteNombre,
                    d.PersonalNombreCompleto,
                    d.Dni,
                    d.CategoriaNombre,
                    d.ValorHora,
                    d.SueldoBasicoNormal,
                    d.TotalHorasNormales,
                    d.ItemHorasExtras,
                    d.AniosAntiguedad,
                    d.ItemAntiguedad,
                    d.ItemFeriados,
                    d.ItemZonaDesfavorable,
                    d.TotalAPagar,
                    d.PdfContenido != null
                ))
                .ToList()
        );
}
