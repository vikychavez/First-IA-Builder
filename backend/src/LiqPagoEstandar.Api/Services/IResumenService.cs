using LiqPagoEstandar.Api.DTOs;

namespace LiqPagoEstandar.Api.Services;

public interface IResumenService
{
    Task<ResumenMensualDto> GenerarAsync(int anio, int mes);
    Task<ResumenMensualDto?> GetAsync(int anio, int mes);
    Task<(byte[] Contenido, string NombreArchivo)?> GetPdfAsync(int anio, int mes, int personalId);
    Task<EnviarResumenResultado> EnviarAsync(int anio, int mes);
}
