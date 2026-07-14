namespace LiqPagoEstandar.Api.Services;

public record AdjuntoEmail(string NombreArchivo, byte[] Contenido);

public interface IEmailService
{
    Task EnviarResumenAsync(string destinatarioEmail, string clienteNombre, int anio, int mes, IReadOnlyList<AdjuntoEmail> adjuntos);
}
