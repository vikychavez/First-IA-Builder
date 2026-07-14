using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace LiqPagoEstandar.Api.Services;

public class EmailService : IEmailService
{
    private static readonly string[] NombresMeses =
    [
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
    ];

    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task EnviarResumenAsync(
        string destinatarioEmail,
        string clienteNombre,
        int anio,
        int mes,
        IReadOnlyList<AdjuntoEmail> adjuntos)
    {
        var smtpSection = _configuration.GetSection("Smtp");
        var periodo = $"{NombresMeses[mes - 1]} {anio}";
        var from = smtpSection["From"] ?? throw new InvalidOperationException("Falta configurar Smtp:From.");
        var host = smtpSection["Host"] ?? throw new InvalidOperationException("Falta configurar Smtp:Host.");

        var mensaje = new MimeMessage();
        mensaje.From.Add(MailboxAddress.Parse(from));
        mensaje.To.Add(MailboxAddress.Parse(destinatarioEmail));
        mensaje.Subject = $"Resumen de Pago - {periodo}";

        var builder = new BodyBuilder
        {
            TextBody = $"Estimado/a {clienteNombre},\n\nAdjuntamos el resumen de pago de su personal correspondiente a {periodo}.\n\nSaludos."
        };

        foreach (var adjunto in adjuntos)
        {
            builder.Attachments.Add(adjunto.NombreArchivo, adjunto.Contenido, ContentType.Parse("application/pdf"));
        }

        mensaje.Body = builder.ToMessageBody();

        using var smtpClient = new SmtpClient();
        var useSsl = smtpSection.GetValue<bool>("UseSsl");
        await smtpClient.ConnectAsync(
            host,
            smtpSection.GetValue<int>("Port"),
            useSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None
        );

        var usuario = smtpSection["User"];
        if (!string.IsNullOrEmpty(usuario))
        {
            await smtpClient.AuthenticateAsync(usuario, smtpSection["Password"] ?? string.Empty);
        }

        await smtpClient.SendAsync(mensaje);
        await smtpClient.DisconnectAsync(true);
    }
}
