using LiqPagoEstandar.Data.Entities;

namespace LiqPagoEstandar.Api.Services;

public interface IPdfService
{
    byte[] GenerarPdf(ResumenPersonalDetalleEntity detalle, int anio, int mes);
    string NombreArchivo(ResumenPersonalDetalleEntity detalle, int anio, int mes);
}
