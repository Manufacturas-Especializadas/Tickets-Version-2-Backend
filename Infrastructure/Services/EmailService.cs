using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class EmailService(
    IOptions<EmailSettings> options,
    ILogger<EmailService> logger) : IEmailService
    {
        private readonly EmailSettings _settings = options.Value;

        public async Task SendNewTicketNotificationAsync(string name, string? department, string? affair, string categoryName)
        {
            var subject = $"Nuevo Ticket Registrado: {affair}";

            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                </head>
                <body style='margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, ""Segoe UI"", Roboto, Helvetica, Arial, sans-serif; background-color: #f8fafc; color: #334155;'>
                    <div style='max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06); border: 1px solid #e2e8f0;'>
                
                        <div style='background-color: #0071ab; padding: 24px; text-align: center;'>
                            <h2 style='color: #ffffff; margin: 0; font-size: 20px; font-weight: 600; letter-spacing: 0.5px;'>Nuevo Ticket en MesaCore</h2>
                        </div>

                        <div style='padding: 32px 24px;'>
                            <p style='margin-top: 0; font-size: 16px; line-height: 24px;'>Hola equipo,</p>
                            <p style='font-size: 16px; line-height: 24px; color: #475569;'>Se ha registrado una nueva solicitud de soporte que requiere su atención. A continuación los detalles:</p>
                    
                            <div style='background-color: #f8fafc; border: 1px solid #e2e8f0; border-radius: 8px; padding: 20px; margin: 24px 0;'>
                                <table width='100%' cellpadding='0' cellspacing='0' style='font-size: 14px;'>
                                    <tr>
                                        <td style='padding: 8px 0; color: #64748b; font-weight: 500; width: 120px;'>Solicitante:</td>
                                        <td style='padding: 8px 0; font-weight: 600; color: #0f172a;'>{name}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 8px 0; color: #64748b; font-weight: 500;'>Departamento:</td>
                                        <td style='padding: 8px 0; color: #0f172a;'>{(string.IsNullOrEmpty(department) ? "N/A" : department)}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 8px 0; color: #64748b; font-weight: 500;'>Asunto:</td>
                                        <td style='padding: 8px 0; color: #0f172a;'>{affair}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 8px 0; color: #64748b; font-weight: 500;'>Categoría:</td>
                                        <td style='padding: 8px 0;'>
                                            <span style='background-color: #e0f2fe; color: #0369a1; padding: 4px 10px; border-radius: 9999px; font-size: 12px; font-weight: 600;'>
                                                {categoryName}
                                            </span>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div style='text-align: center; margin-top: 32px;'>
                                <a href='#' style='background-color: #0071ab; color: #ffffff; padding: 12px 24px; text-decoration: none; border-radius: 6px; font-weight: 600; font-size: 14px; display: inline-block;'>Ver Ticket en el Sistema</a>
                            </div>
                        </div>

                        <div style='background-color: #f1f5f9; padding: 20px; text-align: center; border-top: 1px solid #e2e8f0;'>
                            <p style='margin: 0; font-size: 12px; color: #64748b;'>Este es un mensaje automático generado por el portal MesaCore.<br>Por favor, no respondas a este correo.</p>
                        </div>
                    </div>
                </body>
                </html>";

            var recipients = new List<string>
            {
                "ulises.gonzalez@mesa.ms",
                "juan.poblano@mesa.ms",
                "saul.rodriguez@mesa.ms"
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
                Priority = MailPriority.High,
            };

            foreach (var email in recipients.Where(e => !string.IsNullOrWhiteSpace(e)))
            {
                mailMessage.To.Add(email);
            }

            try
            {
                using var client = new SmtpClient
                {
                    Host = _settings.Host,
                    Port = _settings.Port,
                    EnableSsl = _settings.UseSSL,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_settings.Username, _settings.Password)
                };

                await client.SendMailAsync(mailMessage);
                logger.LogInformation("Correo de notificación de ticket enviado exitosamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocurrió un error al enviar el correo de notificación.");
            }
        }
    }
}